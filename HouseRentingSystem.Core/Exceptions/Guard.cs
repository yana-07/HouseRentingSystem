namespace HouseRentingSystem.Core.Exceptions
{
    public class Guard : IGuard
    {
        public void AgainstNull<T>(T value, string? errorMessage = null)
        {
            if (value == null)
            {
                var ecxeption = errorMessage == null ?
                    new HouseRentingException() :
                    new HouseRentingException(errorMessage);

                throw ecxeption;
            }
        }

        //private T CreateException<T>(string? errorMessage)
        //    where T : class, new()
        //{
        //    T exception = new T();
        //    if (errorMessage != null)
        //    {
        //        exception = new T(errorMessage);
        //    }
        //}
    }
}
