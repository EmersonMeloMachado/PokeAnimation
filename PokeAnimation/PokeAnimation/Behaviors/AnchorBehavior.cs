using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace PokeAnimation.Behaviors
{
    public class AnchorBehavior : Behavior<Xamarin.Forms.View>
    {
        #region Fields

        private PropertyInfo _property;

        private bool alredyDoMax = false;

        private bool alredyDoMin = false;

        public static readonly BindableProperty AnchorToProperty = BindableProperty.Create(
            nameof(AnchorTo),
            typeof(double),
            typeof(AnchorBehavior),
            0D,
            propertyChanged: (b, o, n) =>
            {
                var ab = (AnchorBehavior)b;
                var v = (double)n;
                ab.doBehavior(v);
            }
        );

        public static readonly BindableProperty MaxValueAnchorToProperty = BindableProperty.Create(
            nameof(MaxValueAnchorTo),
            typeof(double),
            typeof(AnchorBehavior),
            -1D);

        public static readonly BindableProperty MinValueAnchorToProperty = BindableProperty.Create(
            nameof(MinValueAnchorTo),
            typeof(double),
            typeof(AnchorBehavior),
            -1D);

        #endregion Fields

        #region Properties

        public double AnchorTo
        {
            get => (double)GetValue(AnchorToProperty);
            set => SetValue(AnchorToProperty, value);
        }

        public double InitialValue
        {
            get;
            set;
        }

        public double MaxValue
        {
            get;
            set;
        }

        public double MaxValueAnchorTo
        {
            get => (double)GetValue(MaxValueAnchorToProperty);
            set => SetValue(MaxValueAnchorToProperty, value);
        }

        public double MinValue
        {
            get;
            set;
        }

        public double MinValueAnchorTo
        {
            get => (double)GetValue(MinValueAnchorToProperty);
            set => SetValue(MinValueAnchorToProperty, value);
        }

        public string PropertyNameToAnchor
        {
            get;
            set;
        }

        public double ProportionalValueAnchorTo
        {
            get;
            set;
        } = 1;

        public Xamarin.Forms.View View
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        private void doBehavior(double v)
        {
            var ab = this;
            if (ab.View != null)
            {
                var d = ab.MaxValue - ab.MinValue;

                var i = 1D;
                if (ab.MinValueAnchorTo > 0 && ab.MaxValueAnchorTo > 0)
                {
                    i = ab.MaxValueAnchorTo - ab.MinValueAnchorTo;
                    i = (d) / i;
                    v = (v - ab.MinValueAnchorTo) * i;
                    v = v + ab.MinValue;
                }

                v = v * ab.ProportionalValueAnchorTo;
                v = v + ab.InitialValue;

                if (v >= ab.MaxValue)
                {
                    if (!ab.alredyDoMax)
                    {
                        if (!ab.View.Behaviors.Any(x => ab.hasConflict(x, v)))
                        {
                            ab._property.SetValue(ab.View, ab.MaxValue);
                        }
                        ab.alredyDoMax = true;
                    }
                    return;
                }
                else if (v <= ab.MinValue)
                {
                    if (!ab.alredyDoMin)
                    {
                        if (!ab.View.Behaviors.Any(x => ab.hasConflict(x, v)))
                        {
                            ab._property.SetValue(ab.View, ab.MinValue);
                        }
                        ab.alredyDoMin = true;
                    }
                    return;
                }

                ab.alredyDoMax = false;
                ab.alredyDoMin = false;
                if (!ab.View.Behaviors.Any(x => ab.hasConflict(x, v)))
                {
                    ab._property.SetValue(ab.View, v);
                }
            }
        }

        private bool hasConflict(Behavior b, double value)
        {
            var bv = b as AnchorBehavior;

            if (bv == null)
            {
                return false;
            }

            if (bv != this)
            {
                if (bv.PropertyNameToAnchor == this.PropertyNameToAnchor)
                {
                    if (value > this.MaxValueAnchorTo && bv.MinValueAnchorTo >= this.MaxValueAnchorTo)
                    {
                        return true;
                    }
                    if (value < this.MinValueAnchorTo && bv.MaxValueAnchorTo <= this.MinValueAnchorTo)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected override void OnAttachedTo(Xamarin.Forms.View bindable)
        {
            this.View = bindable;

            var type = this.View.GetType();

            var propertyInfo = type.GetProperty(this.PropertyNameToAnchor);

            if (propertyInfo == null)
            {
                throw new Exception($"{nameof(this.PropertyNameToAnchor)} invalid");
            }
            this._property = propertyInfo;

            if (this.MaxValueAnchorTo >= 0 || this.MinValueAnchorTo >= 0)
            {
                if (this.MaxValueAnchorTo < 0)
                {
                    throw new Exception($"{this.MaxValueAnchorTo} invalid");
                }
                if (this.MinValueAnchorTo < 0)
                {
                    throw new Exception($"{this.MinValueAnchorTo} invalid");
                }
            }

            base.OnAttachedTo(bindable);
        }

        #endregion Methods
    }
}