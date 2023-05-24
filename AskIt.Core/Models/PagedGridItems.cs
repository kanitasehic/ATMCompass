namespace ATMCompass.Core.Models
{
    public class PagedGridItems<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalItemsCount { get; set; }
    }
}
