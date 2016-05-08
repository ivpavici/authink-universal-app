using System.Linq;
using AuthinkDEMO.Views.GameViews.PairHalfsTask;
using GalaSoft.MvvmLight.Ioc;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using AuthinkDEMO.Views.GameViews;
using AuthinkDEMO.Model.Queries;
using AuthinkDEMO.Services;
using AuthinkDEMO.Controls;

using rules = AuthinkDEMO.Model.Rules;

namespace AuthinkDEMO.Views
{
    public sealed partial class GameView
    {
        public GameView()
        {
            this.InitializeComponent();

            this.taskQueries    = (ITaskQueries)SimpleIoc.Default.GetInstance(typeof(ITaskQueries));

            this.AddHandler(UIElement.PointerPressedEvent,  new PointerEventHandler(_DraggableElement_PointerPressed), true);
            this.AddHandler(UIElement.PointerReleasedEvent, new PointerEventHandler(_DropPlaceholder_PointerReleased), true);

            SoundServices.Instance.Initialize(this.mediaElement);
            PopUpService.Instance.Initialize(this.PopupStoryboard);
        }
       
        private readonly ITaskQueries    taskQueries;

        private DraggableElement draggingElement;
        private Point            startPosition;

        private bool   isDragging;
        private object draggingElement_content;
        private string taskKey;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GameGrid.Children.Clear();

            var task = taskQueries.GetSingle_byId(GameState.GetTask());
            taskKey = task.Type;

            switch (task.Type)
            {
                case rules::Task.Keys.DetectDifferentItems:
                    if (task.Pictures.Count <= 2)
                    {
                        GameGrid.Children.Add(new DetectDifferentItems_HighDifficulty_UserControl());
                    }
                    else
                    {
                        GameGrid.Children.Add(new DetectDifferentItemsUserCotrol());
                    }
                    break;

                case rules::Task.Keys.DetectColors:
                    GameGrid.Children.Add(new DetectColorsUserControl());
                    break;

                case rules::Task.Keys.ContinueSequence:
                    GameGrid.Children.Add(new ContinueSequenceUserControl());
                    break;
                case rules::Task.Keys.PairHalves:
                    GameGrid.Children.Add(new PairHalfsUserControl());
                    break;

                case rules::Task.Keys.DetectSameItems:
                    GameGrid.Children.Add(new DetectSameItemsUserControl());
                    break;

                case rules::Task.Keys.OrderBySize:
                    GameGrid.Children.Add(new OrderBySizeUserControl());
                    break;

                case rules::Task.Keys.VoiceCommands:
                    GameGrid.Children.Add(new VoiceCommandsUserControl());
                    break;

                case rules::Task.Keys.PairSameItems:
                    GameGrid.Children.Add(new PairSameItemsUserControl());
                    break;
            }
        }
        
        private object FindParentOfElement<T>(DependencyObject element)
        {
            if (element == null) { return null; }
            if(element is T)
            {
                return element;
            }

            return FindParentOfElement<T>(VisualTreeHelper.GetParent(element));
        }

        private void _DraggableElement_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (isDragging)
            {
                CancelDragging();
                return;
            }

            if(!(e.OriginalSource is DependencyObject)) { return; }

            draggingElement = FindParentOfElement<DraggableElement>(e.OriginalSource as DependencyObject) as DraggableElement;

            if (draggingElement == null) { return; }

            if (draggingElement.Content is UIElement)
            {
                InitDraggingElement
                (
                    pointerEventArgs: e,
                    draggableElement: draggingElement
                );
            }

            TransferDraggingContentToDraggingElement
            (
                sourceElement: draggingElement
            );

            isDragging = true;
        }
        private void _DropPlaceholder_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!isDragging || draggingElement_content == null)
            {
                return;
            }

            var dropPlaceholder = FindParentOfElement<DropPlaceholder>(e.OriginalSource as DependencyObject) as DropPlaceholder;

            if (dropPlaceholder == null)
            {
                CancelDragging();
                return;
            }

            if (IsDropSuccessful(dropPlaceholder))
            {
                TransferDraggingContentToDropElement
                (
                    destinationElement: dropPlaceholder
                );

                SoundServices.Instance.Play();

                isDragging = false;
            }
            else
            {
                CancelDragging();
            }
        }
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            if (!isDragging)
            {
                return;
            }

            base.OnPointerMoved(e);
            var position = e.GetCurrentPoint(this.Parent as UIElement).Position;

            draggingContentControl.Margin = new Thickness
            (
                left:   position.X - startPosition.X,
                top:    position.Y - startPosition.Y,
                right:  0,
                bottom: 0
            );
        }
  
        private void InitDraggingElement(PointerRoutedEventArgs pointerEventArgs, DraggableElement draggableElement)
        {
            startPosition               = pointerEventArgs.GetCurrentPoint(draggableElement.Content as UIElement).Position;
            var positionRelativeToParent = pointerEventArgs.GetCurrentPoint(this.Parent as UIElement).Position;

            draggingContentControl.Margin = new Thickness
            (
                left: positionRelativeToParent.X - startPosition.X,
                top:  positionRelativeToParent.Y - startPosition.Y,
                right: 0,
                bottom: 0
            );
        }
        private bool IsDropSuccessful(DropPlaceholder dropedOn)
        {
            if(taskKey == rules::Task.Keys.PairHalves)
            {
                return dropedOn.ExpectedPairId == draggingElement.PairId;
            }
            else if (taskKey == rules::Task.Keys.PairSameItems)
            {
                return dropedOn.ExpectedPairId.ToString() == draggingElement.PairId.ToString();
            }

            return false;
        }
        private void CancelDragging()
        {
            draggingContentControl.Content = null;
            draggingElement.Content        = draggingElement_content;
            draggingElement                = null;
            isDragging                     = false;
        }
        private void TransferDraggingContentToDraggingElement(DraggableElement sourceElement)
        {
            draggingElement                  = sourceElement;
            draggingElement_content          = draggingElement.Content;
            draggingElement.Content          = null;
            draggingContentControl.DataContext = draggingElement.DataContext;
            draggingContentControl.Content     = draggingElement_content;
        }
        private void TransferDraggingContentToDropElement(DropPlaceholder destinationElement)
        {
            draggingContentControl.Content = null;

            //destinationElement.Content = draggingElement_content;
            destinationElement.DropElement(draggingElement_content);
            draggingElement.Visibility = Visibility.Collapsed;
            draggingElement            = null;
        }
        
        private void Popup_continue_OnClick(object sender, RoutedEventArgs e)
        {
            MenuPopup.Visibility = Visibility.Collapsed;
        }
    }
}
