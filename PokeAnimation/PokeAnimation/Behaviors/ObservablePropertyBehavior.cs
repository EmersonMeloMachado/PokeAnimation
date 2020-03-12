using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace PokeAnimation.Behaviors
{
    public class ObservableBehaviorEventArgs : EventArgs
    {
        #region Properties

        public double Proportion { get; set; }

        #endregion Properties

        #region Constructors

        public ObservableBehaviorEventArgs(double proportion)
        {
            this.Proportion = proportion;
        }

        #endregion Constructors
    }

    public class ObservablePropertyBehavior : Behavior<Xamarin.Forms.View>
    {
        #region Fields

        private PropertyInfo _property;

        private bool alredyDoMax = false;

        private bool alredyDoMin = false;

        private PropertyChangedEventHandler propertyChanged = async (s, e) =>
        {
        };

        public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(
            nameof(MaxValue),
            typeof(double),
            typeof(ObservablePropertyBehavior),
            -1D);

        public static readonly BindableProperty MinValueProperty = BindableProperty.Create(
            nameof(MinValue),
            typeof(double),
            typeof(ObservablePropertyBehavior),
            -1D);

        #endregion Fields

        #region Properties

        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public double MinValue
        {
            get => (double)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        public string PropertyName
        {
            get;
            set;
        }

        public Xamarin.Forms.View View
        {
            get;
            private set;
        }

        #endregion Properties

        #region Events

        public event EventHandler<ObservableBehaviorEventArgs> ObservablePropertyChanged;

        #endregion Events

        #region Methods

        private bool hasConflict(Behavior b, double value)
        {
            var bv = b as ObservablePropertyBehavior;

            if (bv == null)
            {
                return false;
            }

            if (bv != this)
            {
                if (bv.PropertyName == this.PropertyName)
                {
                    if (value > this.MaxValue && bv.MinValue >= this.MaxValue)
                    {
                        return true;
                    }
                    if (value < this.MinValue && bv.MaxValue <= this.MinValue)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private async void View_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.PropertyName)
            {
                var v = (double)this._property.GetValue(sender);

                if (v <= this.MinValue)
                {
                    if (!alredyDoMin)
                    {
                        if (!this.View.Behaviors.Any(x => this.hasConflict(x, v)))
                        {
                            this.ObservablePropertyChanged?.Invoke(sender, new ObservableBehaviorEventArgs(0));
                        }
                        alredyDoMin = true;
                    }
                    return;
                }
                if (v >= this.MaxValue)
                {
                    if (!alredyDoMax)
                    {
                        if (!this.View.Behaviors.Any(x => this.hasConflict(x, v)))
                        {
                            this.ObservablePropertyChanged?.Invoke(sender, new ObservableBehaviorEventArgs(1));
                        }
                        alredyDoMax = true;
                    }
                    return;
                }
                var i = this.MaxValue - this.MinValue;
                i = (v - this.MinValue) / i;
                alredyDoMax = false;
                alredyDoMin = false;
                if (!this.View.Behaviors.Any(x => this.hasConflict(x, v)))
                {
                    this.ObservablePropertyChanged?.Invoke(sender, new ObservableBehaviorEventArgs(i));
                }
            }
        }

        protected override void OnAttachedTo(Xamarin.Forms.View bindable)
        {
            this.View = bindable;

            var type = this.View.GetType();

            var propertyInfo = type.GetProperty(this.PropertyName);

            if (propertyInfo == null)
            {
                throw new Exception($"{nameof(this.PropertyName)} invalid");
            }
            this._property = propertyInfo;

            if (this.MaxValue >= 0 || this.MinValue >= 0)
            {
                if (this.MaxValue < 0)
                {
                    throw new Exception($"{this.MaxValue} invalid");
                }
                if (this.MinValue < 0)
                {
                    throw new Exception($"{this.MinValue} invalid");
                }
            }

            this.View.PropertyChanged += View_PropertyChanged;

            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Xamarin.Forms.View bindable)
        {
            this.View.PropertyChanged -= View_PropertyChanged;
        }

        #endregion Methods
    }
}