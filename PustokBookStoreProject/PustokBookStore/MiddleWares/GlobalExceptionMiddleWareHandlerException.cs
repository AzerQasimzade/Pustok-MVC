namespace PustokBookStore.MiddleWares
{
    public class GlobalExceptionMiddleWareHandlerException
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleWareHandlerException(RequestDelegate next)
        {
            _next = next;
        }
       public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
               context.Response.Redirect(e.Message);
            }
        }
    }
}
