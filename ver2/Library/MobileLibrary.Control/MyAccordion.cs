using System;
using Xamarin.Forms;

//https://github.com/AndreiMisiukevich/ExpandableView version 1.0.2

namespace MobileLibrary.Control
{
    public class MyAccordion : StackLayout
    {
        public static readonly BindableProperty ExpandAnimationEasingProperty = BindableProperty.Create(nameof(ExpandAnimationEasing), typeof(Easing), typeof(MyAccordion));

        public static readonly BindableProperty ExpandAnimationLengthProperty = BindableProperty.Create(nameof(ExpandAnimationLength), typeof(uint), typeof(MyAccordion), 250u);

        public const string ExpandAnimationName = nameof(ExpandAnimationName);

        public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(MyAccordion), default(bool), BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) => { ((MyAccordion) bindable).OnIsExpandedChanged(); });

        public static readonly BindableProperty IsTouchToExpandEnabledProperty = BindableProperty.Create(nameof(IsTouchToExpandEnabled), typeof(bool), typeof(MyAccordion), true);

        public static readonly BindableProperty PrimaryViewProperty = BindableProperty.Create(nameof(PrimaryView), typeof(View), typeof(MyAccordion), null, propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((MyAccordion) bindable).SetPrimaryView(oldValue as View);
            ((MyAccordion) bindable).OnTouchHandlerViewChanged();
        });

        public static readonly BindableProperty SecondaryViewHeightRequestProperty = BindableProperty.Create(nameof(SecondaryViewHeightRequest), typeof(double), typeof(MyAccordion), -1.0);

        public static readonly BindableProperty SecondaryViewProperty = BindableProperty.Create(nameof(SecondaryView), typeof(View), typeof(MyAccordion), null,
            propertyChanged: (bindable, oldValue, newValue) => { ((MyAccordion) bindable).SetSecondaryView(oldValue as View); });

        public static readonly BindableProperty StatusProperty = BindableProperty.Create(nameof(Status), typeof(ExpandStatus), typeof(MyAccordion), default(ExpandStatus), BindingMode.OneWayToSource);

        public static readonly BindableProperty TouchHandlerViewProperty = BindableProperty.Create(nameof(TouchHandlerView), typeof(View), typeof(MyAccordion), null,
            propertyChanged: (bindable, oldValue, newValue) => { ((MyAccordion) bindable).OnTouchHandlerViewChanged(); });

        public MyAccordion()
        {
            _defaultTapGesture = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (!IsTouchToExpandEnabled)
                    {
                        return;
                    }

                    IsExpanded = !IsExpanded;
                })
            };
        }

        #region Делегаты

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        #endregion

        #region Поля

        private readonly TapGestureRecognizer _defaultTapGesture;
        private double _endHeight;
        private double _lastVisibleHeight = -1;
        private bool _shouldIgnoreAnimation;
        private double _startHeight;

        #endregion

        #region Свойства

        public View PrimaryView
        {
            get => GetValue(PrimaryViewProperty) as View;
            set => SetValue(PrimaryViewProperty, value);
        }

        public bool IsExpanded
        {
            get => (bool) GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public View TouchHandlerView
        {
            get => GetValue(TouchHandlerViewProperty) as View;
            set => SetValue(TouchHandlerViewProperty, value);
        }

        public bool IsTouchToExpandEnabled
        {
            get => (bool) GetValue(IsTouchToExpandEnabledProperty);
            set => SetValue(IsTouchToExpandEnabledProperty, value);
        }

        public double SecondaryViewHeightRequest
        {
            get => (double) GetValue(SecondaryViewHeightRequestProperty);
            set => SetValue(SecondaryViewHeightRequestProperty, value);
        }

        public uint ExpandAnimationLength
        {
            get => (uint) GetValue(ExpandAnimationLengthProperty);
            set => SetValue(ExpandAnimationLengthProperty, value);
        }

        public Easing ExpandAnimationEasing
        {
            get => (Easing) GetValue(ExpandAnimationEasingProperty);
            set => SetValue(ExpandAnimationEasingProperty, value);
        }

        public ExpandStatus Status
        {
            get => (ExpandStatus) GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public View SecondaryView
        {
            get => GetValue(SecondaryViewProperty) as View;
            set => SetValue(SecondaryViewProperty, value);
        }

        #endregion

        #region Методы

        private void InvokeAnimation()
        {
            RaiseStatusChanged(IsExpanded ? ExpandStatus.Expanding : ExpandStatus.Collapsing);

            if (_shouldIgnoreAnimation)
            {
                RaiseStatusChanged(IsExpanded ? ExpandStatus.Expanded : ExpandStatus.Collapsed);
                SecondaryView.HeightRequest = _endHeight;
                SecondaryView.IsVisible = IsExpanded;
                return;
            }

            new Animation(v => SecondaryView.HeightRequest = v, _startHeight, _endHeight)
                .Commit(SecondaryView, ExpandAnimationName, 16, ExpandAnimationLength, ExpandAnimationEasing, (value, interrupted) =>
                {
                    if (interrupted)
                    {
                        return;
                    }

                    if (!IsExpanded)
                    {
                        SecondaryView.IsVisible = false;
                        RaiseStatusChanged(ExpandStatus.Collapsed);
                        return;
                    }

                    RaiseStatusChanged(ExpandStatus.Expanded);
                });
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            _lastVisibleHeight = -1;
        }

        private void OnIsExpandedChanged()
        {
            if (SecondaryView == null)
            {
                return;
            }

            SecondaryView.SizeChanged -= OnSecondaryViewSizeChanged;

            var isExpanding = SecondaryView.AnimationIsRunning(ExpandAnimationName);
            SecondaryView.AbortAnimation(ExpandAnimationName);

            if (IsExpanded)
            {
                SecondaryView.IsVisible = true;
            }

            _startHeight = 0;
            _endHeight = SecondaryViewHeightRequest >= 0
                ? SecondaryViewHeightRequest
                : _lastVisibleHeight;

            var shouldInvokeAnimation = true;

            if (IsExpanded)
            {
                if (_endHeight <= 0)
                {
                    shouldInvokeAnimation = false;
                    SecondaryView.HeightRequest = -1;
                    SecondaryView.SizeChanged += OnSecondaryViewSizeChanged;
                }
            }
            else
            {
                _lastVisibleHeight = _startHeight = SecondaryViewHeightRequest >= 0
                    ? SecondaryViewHeightRequest
                    : !isExpanding
                        ? SecondaryView.Height
                        : _lastVisibleHeight;
                _endHeight = 0;
            }

            _shouldIgnoreAnimation = Height < 0;

            if (shouldInvokeAnimation)
            {
                InvokeAnimation();
            }
        }

        private void OnTouchHandlerViewChanged()
        {
            var touchHandlerView = TouchHandlerView ?? PrimaryView;
            touchHandlerView?.GestureRecognizers.Remove(_defaultTapGesture);
            PrimaryView?.GestureRecognizers.Remove(_defaultTapGesture);
            touchHandlerView?.GestureRecognizers.Add(_defaultTapGesture);
        }

        private void RaiseStatusChanged(ExpandStatus status)
        {
            Status = status;
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }

        private void SetPrimaryView(View oldView)
        {
            if (oldView != null)
            {
                Children.Remove(oldView);
            }

            Children.Insert(0, PrimaryView);
        }

        private void SetSecondaryView(View oldView)
        {
            if (oldView != null)
            {
                Children.Remove(oldView);
            }

            if (SecondaryView is Layout layout)
            {
                layout.IsClippedToBounds = true;
            }

            Children.Add(SecondaryView);
        }

        #endregion

        #region События

        private void OnSecondaryViewSizeChanged(object sender, EventArgs e)
        {
            if (SecondaryView.Height <= 0)
                return;
            SecondaryView.SizeChanged -= OnSecondaryViewSizeChanged;
            SecondaryView.HeightRequest = 0;
            _endHeight = SecondaryView.Height;
            InvokeAnimation();
        }

        #endregion
    }

    public enum ExpandStatus
    {
        Collapsed,
        Expanding,
        Expanded,
        Collapsing
    }

    public sealed class StatusChangedEventArgs : EventArgs
    {
        public StatusChangedEventArgs(ExpandStatus status)
        {
            Status = status;
        }

        #region Свойства

        public ExpandStatus Status { get; }

        #endregion
    }
}