namespace FurnitureStore.Models
{
    public class tblImage
    {
        public int Images_ID { get; set; }

        public byte[] image { get; set; }

        public int items_ID { get; set; }

        public virtual Item items { get; set; }
    }
}
