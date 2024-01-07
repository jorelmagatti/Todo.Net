
namespace Todo.API.BaseController
{
    public static class ExecuteServiceUtil
    {
        #region Public methods
        public static async Task<T> ExecuteServiceAction<T>(Task<T> task)
        {
            try
            {
                return await task;
            }
            catch (Exception)
            {
                throw new Exception("InternalError 500");
            }
        }

        public static async Task ExecuteServiceAction(Task task)
        {
            try
            {
                await task;
            }
            catch (Exception)
            {
                throw new Exception("InternalError 500");
            }
        }
        #endregion
    }
}
