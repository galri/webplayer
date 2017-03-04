using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Infrastructure
{
    public class FocusTextfieldWhenAppearingBehaviour : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.IsVisibleChanged += AssociatedObject_IsVisibleChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.IsVisibleChanged -= AssociatedObject_IsVisibleChanged;
        }

        private void AssociatedObject_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            AssociatedObject.Dispatcher.BeginInvoke(new Action(() =>
            {
                Keyboard.Focus(AssociatedObject);

            }), System.Windows.Threading.DispatcherPriority.Render);
        }
    }
}
