using MoonriseGames.Toolbox.Extensions;
using UnityEngine;

namespace MoonriseGames.Toolbox.Animations
{
    public class AnimateTransformBuilder : AnimateBuilder<AnimateTransformBuilder>
    {
        private Transform Transform { get; }

        public AnimateTransformBuilder(Transform transform, MonoBehaviour context)
            : base(context) => Transform = transform;

        public AnimateTransformBuilder(MonoBehaviour context)
            : base(context) => Transform = context.transform;

        public override AnimateTransformBuilder Delay(float duration)
        {
            AddDelay(duration);
            return this;
        }

        public AnimateTransformBuilder ScaleTo(float duration, AnimationConfig config, Vector3 to) =>
            Scale(duration, config, Transform.localScale, to);

        public AnimateTransformBuilder ScaleBy(float duration, AnimationConfig config, Vector3 delta) =>
            Scale(duration, config, Transform.localScale, Transform.localScale + delta);

        public AnimateTransformBuilder Scale(float duration, AnimationConfig config, Vector3 from, Vector3 to)
        {
            void OnProgress(float progress)
            {
                if (Transform.IsNull())
                    return;

                Transform.localScale = Vector3.Lerp(from, to, progress);
            }

            AddAnimation(new AnimateBetween(Context, duration, config, OnProgress));
            return this;
        }

        public AnimateTransformBuilder RotateTo(float duration, AnimationConfig config, float to) =>
            Rotate(duration, config, Transform.eulerAngles, Vector3.forward * to, false);

        public AnimateTransformBuilder RotateToLocal(float duration, AnimationConfig config, float to) =>
            Rotate(duration, config, Transform.localEulerAngles, Vector3.forward * to, true);

        public AnimateTransformBuilder Rotate(float duration, AnimationConfig config, float from, float to) =>
            Rotate(duration, config, Vector3.forward * from, Vector3.forward * to, false);

        public AnimateTransformBuilder RotateLocal(float duration, AnimationConfig config, float from, float to) =>
            Rotate(duration, config, Vector3.forward * from, Vector3.forward * to, true);

        public AnimateTransformBuilder RotateTo(float duration, AnimationConfig config, Vector3 to) =>
            Rotate(duration, config, Transform.eulerAngles, to, false);

        public AnimateTransformBuilder RotateToLocal(float duration, AnimationConfig config, Vector3 to) =>
            Rotate(duration, config, Transform.localEulerAngles, to, true);

        public AnimateTransformBuilder Rotate(float duration, AnimationConfig config, Vector3 from, Vector3 to) =>
            Rotate(duration, config, from, to, false);

        public AnimateTransformBuilder RotateLocal(float duration, AnimationConfig config, Vector3 from, Vector3 to) =>
            Rotate(duration, config, from, to, true);

        private AnimateTransformBuilder Rotate(float duration, AnimationConfig config, Vector3 from, Vector3 to, bool isLocal)
        {
            void OnProgress(float progress)
            {
                if (Transform.IsNull())
                    return;

                var angle = Quaternion.Lerp(Quaternion.Euler(from), Quaternion.Euler(to), progress);

                if (isLocal)
                    Transform.localRotation = angle;
                else
                    Transform.rotation = angle;
            }

            AddAnimation(new AnimateBetween(Context, duration, config, OnProgress));
            return this;
        }

        public AnimateTransformBuilder RotateBy(float duration, AnimationConfig config, float delta) =>
            RotateEuler(duration, config, Transform.eulerAngles, Transform.eulerAngles.AddZ(delta), false);

        public AnimateTransformBuilder RotateByLocal(float duration, AnimationConfig config, float delta) =>
            RotateEuler(duration, config, Transform.localEulerAngles, Transform.localEulerAngles.AddZ(delta), true);

        public AnimateTransformBuilder RotateBy(float duration, AnimationConfig config, Vector3 delta) =>
            RotateEuler(duration, config, Transform.eulerAngles, Transform.eulerAngles + delta, false);

        public AnimateTransformBuilder RotateByLocal(float duration, AnimationConfig config, Vector3 delta) =>
            RotateEuler(duration, config, Transform.localEulerAngles, Transform.localEulerAngles + delta, true);

        private AnimateTransformBuilder RotateEuler(float duration, AnimationConfig config, Vector3 from, Vector3 to, bool isLocal)
        {
            void OnProgress(float progress)
            {
                if (Transform.IsNull())
                    return;

                if (isLocal)
                    Transform.localEulerAngles = Vector3.Lerp(from, to, progress);
                else
                    Transform.eulerAngles = Vector3.Lerp(from, to, progress);
            }

            AddAnimation(new AnimateBetween(Context, duration, config, OnProgress));
            return this;
        }

        public AnimateTransformBuilder Rotate(AnimationConfig config, float velocity) => Rotate(config, Vector3.forward * velocity, false);

        public AnimateTransformBuilder RotateLocal(AnimationConfig config, float velocity) =>
            Rotate(config, Vector3.forward * velocity, true);

        public AnimateTransformBuilder Rotate(AnimationConfig config, Vector3 velocity) => Rotate(config, velocity, false);

        public AnimateTransformBuilder RotateLocal(AnimationConfig config, Vector3 velocity) => Rotate(config, velocity, true);

        private AnimateTransformBuilder Rotate(AnimationConfig config, Vector3 velocity, bool isLocal)
        {
            var initialAngle = isLocal ? Transform.localRotation : Transform.rotation;

            void OnReset()
            {
                if (Transform.IsNull())
                    return;

                if (isLocal)
                    Transform.localRotation = initialAngle;
                else
                    Transform.rotation = initialAngle;
            }

            void OnProgress(float progress)
            {
                if (Transform.IsNull())
                    return;

                Transform.Rotate(velocity * progress, isLocal ? Space.Self : Space.World);
            }

            AddAnimation(new AnimateContinuous(Context, config, OnReset, OnProgress));
            return this;
        }

        public AnimateTransformBuilder MoveTo(float duration, AnimationConfig config, Vector3 to) =>
            Move(duration, config, Transform.position, to, false);

        public AnimateTransformBuilder MoveToLocal(float duration, AnimationConfig config, Vector3 to) =>
            Move(duration, config, Transform.localPosition, to, true);

        public AnimateTransformBuilder MoveBy(float duration, AnimationConfig config, Vector3 delta) =>
            Move(duration, config, Transform.position, Transform.position + delta, false);

        public AnimateTransformBuilder MoveByLocal(float duration, AnimationConfig config, Vector3 delta) =>
            Move(duration, config, Transform.localPosition, Transform.localPosition + delta, true);

        public AnimateTransformBuilder Move(float duration, AnimationConfig config, Vector3 from, Vector3 to) =>
            Move(duration, config, from, to, false);

        public AnimateTransformBuilder MoveLocal(float duration, AnimationConfig config, Vector3 from, Vector3 to) =>
            Move(duration, config, from, to, true);

        private AnimateTransformBuilder Move(float duration, AnimationConfig config, Vector3 from, Vector3 to, bool isLocal)
        {
            void OnProgress(float progress)
            {
                if (Transform.IsNull())
                    return;

                if (isLocal)
                    Transform.localPosition = Vector3.Lerp(from, to, progress);
                else
                    Transform.position = Vector3.Lerp(from, to, progress);
            }

            AddAnimation(new AnimateBetween(Context, duration, config, OnProgress));
            return this;
        }

        public AnimateTransformBuilder Move(AnimationConfig config, Vector3 velocity) => Move(config, velocity, false);

        public AnimateTransformBuilder MoveLocal(AnimationConfig config, Vector3 velocity) => Move(config, velocity, true);

        private AnimateTransformBuilder Move(AnimationConfig config, Vector3 velocity, bool isLocal)
        {
            var initialPosition = isLocal ? Transform.localPosition : Transform.position;

            void OnReset()
            {
                if (Transform.IsNull())
                    return;

                if (isLocal)
                    Transform.localPosition = initialPosition;
                else
                    Transform.position = initialPosition;
            }

            void OnProgress(float progress)
            {
                if (Transform.IsNull())
                    return;

                Transform.Translate(velocity * progress, isLocal ? Space.Self : Space.World);
            }

            AddAnimation(new AnimateContinuous(Context, config, OnReset, OnProgress));
            return this;
        }
    }
}
