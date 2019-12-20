namespace DFC.FutureAccessModel.AreaRouting.Helpers
{
    public static class It
    {
        public static bool IsNull<TEntity>(TEntity theItem)
            where TEntity : class => theItem is null;
    }
}
