using SICER.MODEL;
using System;

namespace SICER.EXCEPTION
{
    public class CustomException : Exception
    {
        public MessageTypeException MessageTypeException;
        public String Action { get; set; }
        public String Controller { get; set; }

        public CustomException() { }

        public CustomException(string message) : base(message) { }

        public CustomException(string message, Exception inner) : base(message, inner) { }

        public CustomException(TempDataEntityException userException, DataContext DataContext)
            : base(userException.Mensaje)
        {
            MessageTypeException = userException.TipoMensaje;
            ExceptionHelper.LogException(this.GetBaseException(), DataContext);
        }

        public CustomException(TempDataEntityException userException, DataContext DataContext, String Action, String Controller)
            : base(userException.Mensaje)
        {
            MessageTypeException = userException.TipoMensaje;
            this.Action = Action;
            this.Controller = Controller;
            ExceptionHelper.LogException(this.GetBaseException(), DataContext);
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
