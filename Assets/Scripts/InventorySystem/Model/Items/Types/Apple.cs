using InventorySystem.Abstract;

namespace InventorySystem.Model.Items.Types
{
    public class Apple : IInventoryItem
    {
        public TypeItem ItemType { get; }
        public IInventoryItemInfo Info { get; }
        public IInventoryItemState State { get; }

        public Apple(IInventoryItemInfo info)
        {
            this.Info = info;
            State = new InventoryItemState();
        }

        public IInventoryItem Clone()
        {
            var clonedApple = new Apple(Info);
            clonedApple.State.Amount = State.Amount;
            return clonedApple;
        }
    }
}