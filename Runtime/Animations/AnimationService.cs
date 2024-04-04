using MoonriseGames.Toolbox.Architecture;

namespace MoonriseGames.Toolbox.Animations
{
    public class AnimationService : SingletonGlobal<AnimationService>
    {
        // public Animate Scale(Transform target, AnimationConfig config, Vector3 b) => Scale(target, config, target.localScale, b);
        //
        // public Animate Scale(Transform target, AnimationConfig config, Vector3 a, Vector3 b) {
        //     void OnUpdate(float p) => target.localScale = Vector3.Lerp(a, b, p);
        //     return new Animate(this, config, OnUpdate);
        // }
        //
        // public Animate Rotation(Transform target, AnimationConfig config, Quaternion b) =>
        //     Rotation(target, config, target.rotation, b);
        //
        // public Animate Rotation(Transform target, AnimationConfig config, Quaternion a, Quaternion b) {
        //     void OnUpdate(float p) => target.rotation = Quaternion.Lerp(a, b, p);
        //     return new Animate(this, config, OnUpdate);
        // }
        //
        // public Animate RotationLocal(Transform target, AnimationConfig config, Quaternion b) =>
        //     RotationLocal(target, config, target.localRotation, b);
        //
        // public Animate RotationLocal(Transform target, AnimationConfig config, Quaternion a, Quaternion b) {
        //     void OnUpdate(float p) => target.localRotation = Quaternion.Lerp(a, b, p);
        //     return new Animate(this, config, OnUpdate);
        // }
        //
        // public Animate RotationInfinite(Transform target, AnimationConfig config, Vector3 axis, float speed) {
        //     var init = target.eulerAngles;
        //     void OnReset() => target.eulerAngles = init;
        //     void OnUpdate(float p) => target.Rotate(axis, speed * p, Space.World);
        //     return new Animate(this, config, OnReset, OnUpdate);
        // }
        //
        // public Animate RotationInfiniteLocal(Transform target, AnimationConfig config, Vector3 axis, float speed) {
        //     var init = target.localEulerAngles;
        //     void OnReset() => target.localEulerAngles = init;
        //     void OnUpdate(float p) => target.Rotate(axis, speed * p, Space.Self);
        //     return new Animate(this, config, OnReset, OnUpdate);
        // }
        //
        // public Animate Translation(Transform target, AnimationConfig config, Vector3 b) =>
        //     Translation(target, config, target.position, b);
        //
        // public Animate Translation(Transform target, AnimationConfig config, Vector3 a, Vector3 b) {
        //     void OnUpdate(float p) => target.position = Vector3.Lerp(a, b, p);
        //     return new Animate(this, config, OnUpdate);
        // }
        //
        // public Animate TranslationLocal(Transform target, AnimationConfig config, Vector3 b) =>
        //     TranslationLocal(target, config, target.localPosition, b);
        //
        // public Animate TranslationLocal(Transform target, AnimationConfig config, Vector3 a, Vector3 b) {
        //     void OnUpdate(float p) => target.localPosition = Vector3.Lerp(a, b, p);
        //     return new Animate(this, config, OnUpdate);
        // }
        //
        // public Animate TranslationAnchor(RectTransform target, AnimationConfig config, Vector3 b) =>
        //     TranslationAnchor(target, config, target.anchoredPosition, b);
        //
        // public Animate TranslationAnchor(RectTransform target, AnimationConfig config, Vector3 a, Vector3 b) {
        //     void OnUpdate(float p) => target.anchoredPosition = Vector3.Lerp(a, b, p);
        //     return new Animate(this, config, OnUpdate);
        // }
        //
        // public Animate TranslationInfinite(Transform target, AnimationConfig config, Vector3 dir, float speed) {
        //     var init = target.position;
        //     void OnReset() => target.position = init;
        //     void OnUpdate(float p) => target.Translate(dir * speed * p, Space.World);
        //     return new Animate(this, config, OnReset, OnUpdate);
        // }
        //
        // public Animate TranslationInfiniteLocal(Transform target, AnimationConfig config, Vector3 dir, float speed) {
        //     var init = target.localPosition;
        //     void OnReset() => target.localPosition = init;
        //     void OnUpdate(float p) => target.Translate(dir * speed * p, Space.Self);
        //     return new Animate(this, config, OnReset, OnUpdate);
        // }
        //
        // public Animate Delay(float duration) => new(this, new AnimationConfig(duration, AnimationInterpolation.Linear), null);
    }
}
