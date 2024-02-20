using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NKStudio
{
    [CustomEditor(typeof(CAAController))]
    public class CAAControllerEditor : Editor
    {
        private VisualTreeAsset _template;
        private VisualElement _root;

        private Label _title;
        private Toggle _autoVolumeToggle;
        private ObjectField _targetField;
        private Slider _redOffsetX;
        private Slider _redOffsetY;
        private Slider _greenOffsetX;
        private Slider _greenOffsetY;
        private Slider _blueOffsetX;
        private Slider _blueOffsetY;

        private SerializedProperty _autoVolume;
        private SerializedProperty _redOffset;
        private SerializedProperty _greenOffset;
        private SerializedProperty _blueOffset;

        private void FindProperty()
        {
            _autoVolume = serializedObject.FindProperty("AutoVolume");
            _redOffset = serializedObject.FindProperty("RedOffset");
            _greenOffset = serializedObject.FindProperty("GreenOffset");
            _blueOffset = serializedObject.FindProperty("BlueOffset");
        }

        private void InitElement()
        {
            _root = _template.CloneTree();
            _title = _root.Q<Label>("Title");
            _autoVolumeToggle = _root.Q<Toggle>("Auto-Volume-Toggle");
            _targetField = _root.Q<ObjectField>("Volume-Field");
            _redOffsetX = _root.Q<Slider>("Red-Offset-X");
            _redOffsetY = _root.Q<Slider>("Red-Offset-Y");
            _greenOffsetX = _root.Q<Slider>("Green-Offset-X");
            _greenOffsetY = _root.Q<Slider>("Green-Offset-Y");
            _blueOffsetX = _root.Q<Slider>("Blue-Offset-X");
            _blueOffsetY = _root.Q<Slider>("Blue-Offset-Y");

            // 값 바인딩
            _redOffsetX.value = _redOffset.vector2Value.x;
            _redOffsetY.value = _redOffset.vector2Value.y;
            _greenOffsetX.value = _greenOffset.vector2Value.x;
            _greenOffsetY.value = _greenOffset.vector2Value.y;
            _blueOffsetX.value = _blueOffset.vector2Value.x;
            _blueOffsetY.value = _blueOffset.vector2Value.y;

            _autoVolumeToggle.RegisterValueChangedCallback(evt => {
                _autoVolume.boolValue = evt.newValue;
                serializedObject.ApplyModifiedProperties();

                if (evt.newValue)
                    _targetField.style.display = DisplayStyle.None;
                else
                    _targetField.style.display = DisplayStyle.Flex;
            });
        }

        private void OnEnable()
        {
            string path = AssetDatabase.GUIDToAssetPath("b7ad56b577b704eb8859915568b7f4d9");
            _template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        }

        public override VisualElement CreateInspectorGUI()
        {
            FindProperty();
            InitElement();

            _title.RegisterCallback<ClickEvent>(evt => OpenBehaviour(target as MonoBehaviour));

            _redOffsetX.RegisterValueChangedCallback(evt => {
                _redOffset.vector2Value = new Vector2(evt.newValue, _redOffset.vector2Value.y);
                serializedObject.ApplyModifiedProperties();
            });

            _redOffsetY.RegisterValueChangedCallback(evt => {
                _redOffset.vector2Value = new Vector2(_redOffset.vector2Value.x, evt.newValue);
                serializedObject.ApplyModifiedProperties();
            });

            _greenOffsetX.RegisterValueChangedCallback(evt => {
                _greenOffset.vector2Value = new Vector2(evt.newValue, _greenOffset.vector2Value.y);
                serializedObject.ApplyModifiedProperties();
            });

            _greenOffsetY.RegisterValueChangedCallback(evt => {
                _greenOffset.vector2Value = new Vector2(_greenOffset.vector2Value.x, evt.newValue);
                serializedObject.ApplyModifiedProperties();
            });

            _blueOffsetX.RegisterValueChangedCallback(evt => {
                _blueOffset.vector2Value = new Vector2(evt.newValue, _blueOffset.vector2Value.y);
                serializedObject.ApplyModifiedProperties();
            });

            _blueOffsetY.RegisterValueChangedCallback(evt => {
                _blueOffset.vector2Value = new Vector2(_blueOffset.vector2Value.x, evt.newValue);
                serializedObject.ApplyModifiedProperties();
            });

            return _root;
        }

        private static void OpenBehaviour(MonoBehaviour targetBehaviour)
        {
            MonoScript scriptAsset = MonoScript.FromMonoBehaviour(targetBehaviour);
            string path = AssetDatabase.GetAssetPath(scriptAsset);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            AssetDatabase.OpenAsset(textAsset);
        }
    }
}