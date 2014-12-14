using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;

namespace Homie.Common.WPF
{
    static public class ListViewSorter
    {

        public static DependencyProperty CustomSorterProperty = DependencyProperty.RegisterAttached(
         "CustomSorter",
         typeof(IComparer),
         typeof(ListViewSorter));

        public static IComparer GetCustomSorter(DependencyObject obj)
        {
            return (IComparer)obj.GetValue(CustomSorterProperty);
        }

        public static void SetCustomSorter(DependencyObject obj, IComparer value)
        {
            obj.SetValue(CustomSorterProperty, value);
        }

        public static DependencyProperty SortBindingMemberProperty = DependencyProperty.RegisterAttached(
            "SortBindingMember",
            typeof(BindingBase),
            typeof(ListViewSorter));

        public static BindingBase GetSortBindingMember(DependencyObject obj)
        {
            return (BindingBase)obj.GetValue(SortBindingMemberProperty);
        }

        public static void SetSortBindingMember(DependencyObject obj, BindingBase value)
        {
            obj.SetValue(SortBindingMemberProperty, value);
        }

        public readonly static DependencyProperty IsListviewSortableProperty = DependencyProperty.RegisterAttached(
            "IsListviewSortable",
            typeof(Boolean),
            typeof(ListViewSorter),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnRegisterSortableGrid)));

        public static Boolean GetIsListviewSortable(DependencyObject obj)
        {
            //return true;
            return (Boolean)obj.GetValue(IsListviewSortableProperty);
        }

        public static void SetIsListviewSortable(DependencyObject obj, Boolean value)
        {
            obj.SetValue(IsListviewSortableProperty, value);
        }

        private static GridViewColumnHeader _lastHeaderClicked = null;
        private static ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private static ListView lv = null;

        private static void OnRegisterSortableGrid(DependencyObject obj,
          DependencyPropertyChangedEventArgs args)
        {
            ListView grid = obj as ListView;
            if (grid != null)
            {
                lv = grid;
                RegisterSortableGridView(grid, args);
            }
        }

        private static void RegisterSortableGridView(ListView grid,
          DependencyPropertyChangedEventArgs args)
        {

            if (args.NewValue is Boolean && (Boolean)args.NewValue)
            {
                grid.AddHandler(GridViewColumnHeader.ClickEvent,
                    new RoutedEventHandler(GridViewColumnHeaderClickedHandler));
            }
            else
            {
                grid.AddHandler(GridViewColumnHeader.ClickEvent,
                 new RoutedEventHandler(GridViewColumnHeaderClickedHandler));
            }
        }


        private static void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {

            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked != _lastHeaderClicked)
                {
                    direction = ListSortDirection.Ascending;
                }

                else
                {
                    if (_lastDirection == ListSortDirection.Ascending)
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        direction = ListSortDirection.Ascending;
                    }

                }

                string header = String.Empty;

                try
                {
                    header = ((Binding)GetSortBindingMember(headerClicked.Column)).Path.Path;
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }

                if (header == String.Empty)
                    return;

                Sort(header, direction);

                string resourceTemplateName = String.Empty;
                DataTemplate tmpTemplate;

                // Clear header template if neccessary (removes the sort arrow)
                if (_lastHeaderClicked != null)
                {
                    resourceTemplateName = "HeaderTemplateSortNon";
                    tmpTemplate = lv.TryFindResource(resourceTemplateName) as DataTemplate;
                    _lastHeaderClicked.Column.HeaderTemplate = tmpTemplate;
                }

                switch (direction)
                {
                    case ListSortDirection.Ascending: resourceTemplateName = "HeaderTemplateSortAsc"; break;
                    case ListSortDirection.Descending: resourceTemplateName = "HeaderTemplateSortDesc"; break;
                }

                tmpTemplate = lv.TryFindResource(resourceTemplateName) as DataTemplate;
                if (tmpTemplate != null)
                {
                    headerClicked.Column.HeaderTemplate = tmpTemplate;
                }

                _lastHeaderClicked = headerClicked;
                _lastDirection = direction;
            }

        }

        private static void Sort(string sortBy, ListSortDirection direction)
        {
            ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(lv.ItemsSource);

            if (view != null)
            {
                try
                {
                    ListViewCustomComparer sorter = (ListViewCustomComparer)ListViewSorter.GetCustomSorter(lv);
                    if (sorter != null)
                    {
                        sorter.SortColumn = sortBy;
                        sorter.SortDirection = direction;

                        view.CustomSort = sorter;
                        lv.Items.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }

            }
        }
    }


}