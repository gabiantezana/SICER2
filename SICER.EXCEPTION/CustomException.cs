using SICER.MODEL;
using System;

namespace SICER.EXCEPTION
{
    public sealed class CustomException : Exception
    {
        public MessageTypeException MessageTypeException;
        public String Action { get; set; }
        public String Controller { get; set; }

        public CustomException() { }

        public CustomException(string message) : base(message) { }

        public CustomException(string message, Exception inner) : base(message, inner) { }

        public CustomException(TempDataEntityException userException, DataContext dataContext)
            : base(userException.Mensaje)
        {
            MessageTypeException = userException.TipoMensaje;
            ExceptionHelper.LogException(this.GetBaseException(), dataContext);
        }

        public CustomException(TempDataEntityException userException, DataContext dataContext, string action, string controller)
            : base(userException.Mensaje)
        {
            MessageTypeException = userException.TipoMensaje;
            this.Action = action;
            this.Controller = controller ?? throw new ArgumentNullException(nameof(controller));
            ExceptionHelper.LogException(this.GetBaseException(), dataContext);
        }
    }


    //Para evitar referencia circular entre librerias
    public class TempDataEntityException
    {
        public MessageTypeException TipoMensaje { get; set; }
        public String Mensaje { get; set; }
    }

    public enum MessageTypeException
    {
        Success,
        Warning,
        Info,
        Error
    }
}
