using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiAppCrud.Utilities
{
    public class EmpleadoMensajeria : ValueChangedMessage<EmpleadoMensaje>
    {
        public EmpleadoMensajeria(EmpleadoMensaje value) : base(value)
        {
        }
    }
}
