using System.Collections.Generic;
using System.Linq;
using Code.UI.InventoryViewModel.Item;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InventoryViewModel.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [Space(10)] [Header("Main")] 
        [SerializeField] private RectTransform _slotsContainer;
        [SerializeField] private RectTransform _itemsContainer;
        [SerializeField] private RectTransform _itemDragContainer;
        [Space(10)] [Header("Additional")] 
        [SerializeField] private DestroyerItemHolder _destroyItemHolder;
        [SerializeField] private FreeAreaItemHolder _freeAreaItemHolder;
        [Space(10)] [Header("Animation")] 
        [SerializeField] private Image _bg;
        [SerializeField] private Transform _mainPanel;

        private IInventoryViewModel _inventoryVM;

        private void OnValidate()
        {
            if (_destroyItemHolder == null)
                _destroyItemHolder.GetComponentInChildren<DestroyerItemHolder>();

            if (_freeAreaItemHolder == null)
                _freeAreaItemHolder.GetComponentInChildren<FreeAreaItemHolder>();
        }

        public void Initialize(IInventoryViewModel inventoryM)
        {
            _inventoryVM = inventoryM;

            _destroyItemHolder.Initialize(_inventoryVM);
            _freeAreaItemHolder.Initialize(_inventoryVM);
        }

        public void Dispose()
        {
            _destroyItemHolder.Dispose();
            _freeAreaItemHolder.Dispose();

            Destroy(this.gameObject);
        }

        public RectTransform SlotsContainer => _slotsContainer;
        public RectTransform ItemsContainer => _itemsContainer;
        public RectTransform DestroyItemContainer => _destroyItemHolder.ContainerHolder;
        public RectTransform FreeAreaItemContainer => _freeAreaItemHolder.ContainerHolder;
        public RectTransform ItemDragContainer => _itemDragContainer;

        public void PlayAnimationShow()
        {
            List<SlotContainer> slotViews = _inventoryVM.GetSlotContainers();
            List<ItemView> itemViews = _inventoryVM.GetItemViews();
            
            SetUpStartAnimationComponents(slotViews, itemViews);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_bg.DOFade(1f, 0.25f).SetEase(Ease.Linear));
            sequence.Append(_mainPanel.transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutElastic));
            sequence.AppendInterval(0.1f);
            AnimateSlotWave(sequence, slotViews);
            sequence.AppendInterval(0.1f);
            AnimateItemWave(sequence, itemViews);
            sequence.AppendInterval(0.1f);
            AnimateAdditionalHolders(sequence);
        }

        private void SetUpStartAnimationComponents(List<SlotContainer> slotViews, List<ItemView> itemViews)
        {
            _bg.color = new Color(1f, 1f, 1f, 0f);
            _mainPanel.transform.localScale = new Vector3(1f, 0f, 1f);
            _destroyItemHolder.transform.localScale = Vector3.zero;
            _freeAreaItemHolder.transform.localScale = Vector3.zero;

            slotViews.ForEach(x => x.View.transform.localScale = Vector3.zero);
            itemViews.ForEach(x => x.transform.localScale = Vector3.zero);
        }

        private void AnimateSlotWave(Sequence sequence, List<SlotContainer> slotViews)
        {
            List<IGrouping<int, SlotContainer>> diagonalGroups = slotViews
                .GroupBy(slot => slot.ViewModel.GridCell.GridX + slot.ViewModel.GridCell.GridY)
                .OrderBy(group => group.Key)
                .ToList();

            float slotWaveStep = 0.02f;

            for (int groupIndex = 0; groupIndex < diagonalGroups.Count; groupIndex++)
            {
                IGrouping<int, SlotContainer> group = diagonalGroups[groupIndex];
                foreach (var slot in group)
                {
                    sequence.Join(slot.View.transform.DOScale(1f, 0.25f)
                        .SetEase(Ease.OutBack)
                        .SetDelay(slotWaveStep));
                }
            }
        }

        private void AnimateItemWave(Sequence sequence, List<ItemView> itemViews)
        {
            float itemDelayStep = 0.02f;
            for (int i = 0; i < itemViews.Count; i++)
            {
                ItemView item = itemViews[i];
                sequence.Join(item.transform.DOScale(1f, 0.25f)
                    .SetEase(Ease.OutBack)
                    .SetDelay(itemDelayStep * i));
            }
        }

        private void AnimateAdditionalHolders(Sequence sequence)
        {
            sequence.AppendCallback(() =>
            {
                _destroyItemHolder.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
                _freeAreaItemHolder.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
            });
        }
    }
}