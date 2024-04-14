//Event that informs subscribers of a debug log

using Paints.PaintItems;

namespace Events
{
    public class SetPlayerPaintQuantity : EventData
    {
        public readonly int Id;
        public readonly float Quantity;

        public SetPlayerPaintQuantity(int id, float quantity) : base(
            EventIdentifiers.SetPlayerPaintQuantity)
        {
            Id = id;
            Quantity = quantity;
        }
    }
    
    public class WishlistPaint : EventData
    {
        public readonly int Id;
        public WishlistPaint(int id) : base(EventIdentifiers.WishlistPaint)
        {
            Id = id;
        }
    }
    
    public class RequestPaintData : EventData
    {
        public readonly int Id;
        public RequestPaintData(int id) : base(EventIdentifiers.RequestPaintData)
        {
            Id = id;
        }
    }
    
    public class OpenPaintContextMenu : EventData
    {
        public readonly PaintData PaintData;
        public readonly float Quantity;

        public OpenPaintContextMenu(PaintData paintData, float quantity) : base(EventIdentifiers.OpenPaintContextMenu)
        {
            PaintData = paintData;
            Quantity = quantity;
        }
    }
}
