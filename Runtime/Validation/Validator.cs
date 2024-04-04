using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MoonriseGames.Toolbox.Constants;
using MoonriseGames.Toolbox.Extensions;
using UnityEngine;
using UnityEngine.Rendering;

namespace MoonriseGames.Toolbox.Validation
{
    //TODO: mention ignore options including tags in doc
    public class Validator
    {
        private const BindingFlags FIELD_BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public ValidationResult Result { get; } = new();
        private HashSet<object> ValidatedObjects { get; } = new();

        public void Validate(object target) => Validate(target, target.GetType().Name);

        private void Validate(object target, string path)
        {
            if (IsValidationRequired(target).Not())
                return;

            RegisterValidation(target);

            ValidateValidateable(target, path);
            ValidateFieldValues(target, path);

            ValidateSortingGroupLayers(target, path);
        }

        private void ValidateValidateable(object target, string path)
        {
            if (target is not IValidateable validateable)
                return;

            try
            {
                validateable.Validate();
            }
            catch (ValidationException e)
            {
                Result.AddIssue(path, e.Message);
            }
        }

        private void ValidateFieldValues(object target, string path)
        {
            foreach (var field in target.GetType().GetFields(FIELD_BINDING_FLAGS))
                ValidateFieldValue(field, field.GetValue(target), ExtendPath(path, field.Name));
        }

        private void ValidateFieldValue(MemberInfo field, object value, string path)
        {
            if (field.IsDefined(typeof(SerializeField)).Not())
                return;

            if (field.IsDefined(typeof(NoValidationAttribute)))
                return;

            if (value.IsNull() && field.IsDefined(typeof(OptionalAttribute)))
                return;

            if (value.IsNull())
            {
                Result.AddIssue(path, "Missing required field value");
                return;
            }

            if (value is object[] array)
                ValidateCollectionValues(array, path);

            if (value is List<object> list)
                ValidateCollectionValues(list, path);

            Validate(value, path);
        }

        private void ValidateCollectionValues(IEnumerable<object> iterator, string path)
        {
            foreach (var (item, index) in iterator.Indexed())
            {
                var itemPath = ExtendPath(path, $"[{index}]");

                if (item.IsNull())
                {
                    Result.AddIssue(itemPath, "Missing collection item value");
                    continue;
                }

                Validate(item, itemPath);
            }
        }

        private void ValidateSortingGroupLayers(object target, string path)
        {
            if (target is not SortingGroup group)
                return;

            if (group.sortingLayerName == "<unknown layer>")
                Result.AddIssue(ExtendPath(path, "sortingLayerName"), "Invalid sorting layer");
        }

        private bool IsValidationRequired(object target)
        {
            if (ValidatedObjects.Contains(target))
                return false;

            if (IsInIgnoredNamespace(target.GetType()))
                return false;

            if (target.GetType().IsDefined(typeof(NoValidationAttribute), false))
                return false;

            if (target is Behaviour { tag: Tags.NO_VALIDATION })
                return false;

            return true;
        }

        private string ExtendPath(string path, string extension) => $"{path}/{extension}";

        private void RegisterValidation(object target) => ValidatedObjects.Add(target);

        private bool IsInIgnoredNamespace(Type type) =>
            type.Namespace != null
            && ValidationCons.IgnoredNamespaces.Any(type.Namespace.StartsWith)
            && ValidationCons.IncludedClasses.Any(type.Name.Equals).Not();
    }
}
