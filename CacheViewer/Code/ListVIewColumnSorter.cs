
using System.Collections;
using System.Windows.Forms;

namespace CacheViewer.Code
{
    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        
        private int columnToSort;
        private SortOrder orderOfSort;
        private readonly CaseInsensitiveComparer objectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            this.columnToSort = 0;

            // Initialize the sort order to 'none'
            this.orderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            this.objectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            // Cast the objects to be compared to ListViewItem objects
            ListViewItem listviewX = (ListViewItem)x;
            ListViewItem listviewY = (ListViewItem)y;

            int xint = int.Parse(listviewX.SubItems[this.columnToSort].Text);
            int yint = int.Parse(listviewY.SubItems[this.columnToSort].Text);

            // Compare the two items
            int compareResult = xint.CompareTo(yint);

            // Calculate correct return value based on object comparison
            if (this.orderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            if (this.orderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            // Return '0' to indicate they are equal
            return 0;
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                this.columnToSort = value;
            }
            get
            {
                return this.columnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                this.orderOfSort = value;
            }
            get
            {
                return this.orderOfSort;
            }
        }

    }
}