using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokeAnimation.Helpers
{
    [ContentProperty(nameof(Bindings))]
    public class MultiBinding : IMarkupExtension<Binding>
    {
        #region Classes

        private sealed class InternalValue : INotifyPropertyChanged
        {
            #region Fields

            private object _value;

            #endregion Fields

            #region Properties

            public object Value
            {
                get { return _value; }
                set
                {
                    if (!Equals(_value, value))
                    {
                        _value = value;
                        OnPropertyChanged();
                    }
                }
            }

            #endregion Properties

            #region Events

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion Events

            #region Methods

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion Methods
        }

        private sealed class MultiValueConverterWrapper : IValueConverter
        {
            #region Fields

            private readonly IMultiValueConverter _multiValueConverter;
            private readonly string _stringFormat;

            #endregion Fields

            #region Constructors

            public MultiValueConverterWrapper(IMultiValueConverter multiValueConverter, string stringFormat)
            {
                _multiValueConverter = multiValueConverter;
                _stringFormat = stringFormat;
            }

            #endregion Constructors

            #region Methods

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (_multiValueConverter != null)
                {
                    value = _multiValueConverter.Convert(value as object[], targetType, parameter, culture);
                }
                if (!string.IsNullOrWhiteSpace(_stringFormat))
                {
                    var array = value as object[];
                    // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
                    if (array != null)
                    {
                        value = string.Format(_stringFormat, array);
                    }
                    else
                    {
                        value = string.Format(_stringFormat, value);
                    }
                }
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            #endregion Methods
        }

        #endregion Classes

        #region Fields

        private readonly InternalValue _internalValue = new InternalValue();
        private readonly IList<BindableProperty> _properties = new List<BindableProperty>();
        private BindableObject _target;

        #endregion Fields

        #region Properties

        public IList<Binding> Bindings { get; } = new List<Binding>();

        public IMultiValueConverter Converter { get; set; }
        public object ConverterParameter { get; set; }
        public string StringFormat { get; set; }

        #endregion Properties

        #region Methods

        private void SetValue()
        {
            if (_target == null) return;
            _internalValue.Value = _properties.Select(_target.GetValue).ToArray();
        }

        public Binding ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(StringFormat) && Converter == null)
                throw new InvalidOperationException($"{nameof(MultiBinding)} requires a {nameof(Converter)} or {nameof(StringFormat)}");

            //Get the object that the markup extension is being applied to
            var provideValueTarget = (IProvideValueTarget)serviceProvider?.GetService(typeof(IProvideValueTarget));
            _target = provideValueTarget?.TargetObject as BindableObject;

            if (_target == null) return null;

            foreach (Binding b in Bindings)
            {
                var property = BindableProperty.Create($"Property-{Guid.NewGuid().ToString("N")}", typeof(object),
                    typeof(MultiBinding), default(object), propertyChanged: (_, o, n) => SetValue());
                _properties.Add(property);
                _target.SetBinding(property, b);
            }
            SetValue();

            var binding = new Binding
            {
                Path = nameof(InternalValue.Value),
                Converter = new MultiValueConverterWrapper(Converter, StringFormat),
                ConverterParameter = ConverterParameter,
                Source = _internalValue
            };

            return binding;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        #endregion Methods

        #region Interfaces

        public interface IMultiValueConverter
        {
            #region Methods

            object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

            #endregion Methods
        }

        #endregion Interfaces
    }
}