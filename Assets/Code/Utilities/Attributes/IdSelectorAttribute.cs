using System;

namespace Code.Utilities.Attributes
{
    public abstract class IdSelectorAttribute : Attribute
    {
        public bool HasGameObjectField { get; set; }
        public string OverrideName { get; set; } = string.Empty;
    }
}