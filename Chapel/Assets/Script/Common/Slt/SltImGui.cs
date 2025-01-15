using UnityEngine;
using ImGuiNET;
using System;

public static class SltImGui
{
    // fracDigit:�����̌���
    // totalDigit:�S�̂̌���
    // -1�Ȃ�f�t�H���g
    public static void TextVector2(string label, in Vector2 vector, int fracDigit = -1, int totalDigit = -1)
    {
        string vecStr = FormatVector2(vector, fracDigit, totalDigit);
        string str = $"{label}:{vecStr}";
        ImGui.Text(str);
    }

    public static string FormatVector2(Vector2 vector, int fracDigits = -1, int totalDigits = -1)
    {
        // �f�t�H���g�l�̏���
        totalDigits = (totalDigits == -1) ? 8 : totalDigits;
        fracDigits = (fracDigits == -1) ? 2 : fracDigits;

        // �t�H�[�}�b�g������𐶐�
        string format = $"{{0,{totalDigits}:F{fracDigits}}}";

        // �t�H�[�}�b�g��K�p
        string xFormatted = string.Format(format, vector.x);
        string yFormatted = string.Format(format, vector.y);

        // ������Ƃ��ĕԂ�
        return $"({xFormatted}, {yFormatted})";
    }

    public static bool EnumValueCombo<TEnum>(ref TEnum enumValue) where TEnum : Enum
    {
        string[] enumNames = Enum.GetNames(typeof(TEnum));
        int currentIndex = Array.IndexOf(enumNames, enumValue.ToString());
        bool valueChanged = false;

        if (ImGui.BeginCombo("Select Enum", enumNames[currentIndex]))
        {
            for (int i = 0; i < enumNames.Length; i++)
            {
                bool isSelected = (i == currentIndex);
                if (ImGui.Selectable(enumNames[i], isSelected))
                {
                    currentIndex = i;
                    enumValue = (TEnum)Enum.Parse(typeof(TEnum), enumNames[i]);
                    valueChanged = true;
                }

                // �I����Ԃ������\��
                if (isSelected)
                {
                    ImGui.SetItemDefaultFocus();
                }
            }
            ImGui.EndCombo();
        }

        return valueChanged;
    }

    public static bool TreeNode(string label)
    {
        return ImGui.TreeNode(label);
    }
    public static void TreePop()
    {
        ImGuiNative.igTreePop();
    }

}
