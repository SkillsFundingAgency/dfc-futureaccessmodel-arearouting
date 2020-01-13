namespace DFC.FutureAccessModel.AreaRouting.Helpers
{
    public static class It
    {
        public static bool IsNull<TEntity>(TEntity theItem)
            where TEntity : class => theItem is null;

        public static bool IsEmpty(string theItem) =>
            string.IsNullOrWhiteSpace(theItem);

        public static bool Has<TEntity>(TEntity theItem)
            where TEntity : class => !IsNull(theItem);

        public static bool Has<TEntity>(TEntity? theItem)
            where TEntity : struct =>
            theItem != null;
    }
}
