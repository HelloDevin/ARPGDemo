using System;

namespace ZZZ
{
    public class BindableProperty<T>
    {
        private T _value;

        public Action<T> OnValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (!value.Equals(_value))
                {
                    _value = value;
                    OnValueChanged?.Invoke(_value);
                }
            }
        }
    }
}