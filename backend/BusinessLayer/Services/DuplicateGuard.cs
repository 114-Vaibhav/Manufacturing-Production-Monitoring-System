using BusinessLayer.Exceptions;

namespace BusinessLayer.Services
{
    internal static class DuplicateGuard
    {
        public static void ThrowIfDuplicate<T>(
            IEnumerable<T>? items,
            Func<T, bool> isDuplicate,
            string entityName)
        {
            // Ignore database timestamps and ids; the meaningful request data must differ.
            if (items != null && items.Any(isDuplicate))
            {
                throw new ValidationException($"{entityName} duplicate entry already exists.");
            }
        }
    }
}
