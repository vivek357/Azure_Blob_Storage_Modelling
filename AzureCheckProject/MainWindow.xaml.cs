using AzureBlobStorageModelling.Logic;
using AzureBlobStorageModelling.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AzureBlobStorageModelling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<BlobItem> _BlobCollection = new ObservableCollection<BlobItem>();
        public ObservableCollection<BlobItem> BlobCollection { get { return _BlobCollection; } }

        public MainWindow()
        {
            InitializeComponent();
            //ShowContainer("new");
            CloudBlobClient cloudBlobClient=Login_Azure.AzureConnection();
            CloudBlobContainer container = cloudBlobClient.GetContainerReference("new");

            foreach (CloudBlobContainer item in cloudBlobClient.ListContainers()) 
            {
                trvStructure.Items.Add(CreateTreeItem(item));
            }
        }


      /*  public void ShowContainer(string containerName)
        {
            CloudBlobClient cloudBlobClient = Login_Azure.AzureConnection();
            CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);

            if (container != null)
            {
                IEnumerable<IListBlobItem> blobs = container.ListBlobs(null, true, BlobListingDetails.All);
                if (blobs != null)
                {
                    foreach (IListBlobItem item in blobs)
                    {
                        try
                        {
                            if (item.GetType() == typeof(CloudBlobDirectory))
                            {
                            }
                            else if (item.GetType() == typeof(CloudBlockBlob))
                            {
                                CloudBlockBlob blockBlob = item as CloudBlockBlob;
                                _BlobCollection.Add(new BlobItem()
                                {
                                    Name = blockBlob.Name,
                                    BlobType = "Block",
                                    ContentType = blockBlob.Properties.ContentType,
                                    Encoding = blockBlob.Properties.ContentEncoding,
                                    Length = blockBlob.Properties.Length,
                                    ETag = blockBlob.Properties.ETag,
                                    LastModified = blockBlob.Properties.LastModified.Value.DateTime,
                                    LastModifiedText = blockBlob.Properties.LastModified.Value.ToString()
                                });
                                            
                                        
                            }
                            else if (item.GetType() == typeof(CloudPageBlob))
                            {
                                CloudPageBlob pageBlob = item as CloudPageBlob;                                                                      
                                _BlobCollection.Add(new BlobItem()
                                {
                                    Name = pageBlob.Name,
                                    BlobType = "Page",
                                    ContentType = pageBlob.Properties.ContentType,
                                    Encoding = pageBlob.Properties.ContentEncoding,
                                    Length = pageBlob.Properties.Length,
                                    ETag = pageBlob.Properties.ETag,
                                    LastModified = pageBlob.Properties.LastModified.Value.DateTime,
                                    LastModifiedText = pageBlob.Properties.LastModified.Value.ToString()
                                });                                                                
                            }
                        }
                        catch (Exception)
                        {

                        }                      
                    }
                    foreach (BlobItem it in BlobCollection)
                        trvStructure.Items.Add(it.Name);
                }
            }
        }*/




        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            if ((item.Items.Count == 1) && (item.Items[0] is string))
            {
                item.Items.Clear();

                

                if (item.Tag is CloudBlockBlob)
                {
                    CloudBlockBlob blockBlob = item.Tag as CloudBlockBlob;
                    _BlobCollection.Add(new BlobItem()
                    {
                        Name = blockBlob.Name,
                        BlobType = "Block",
                        ContentType = blockBlob.Properties.ContentType,
                        Encoding = blockBlob.Properties.ContentEncoding,
                        Length = blockBlob.Properties.Length,
                        ETag = blockBlob.Properties.ETag,
                        LastModified = blockBlob.Properties.LastModified.Value.DateTime,
                        LastModifiedText = blockBlob.Properties.LastModified.Value.ToString()
                    });
                    //CloudPageBlob expandedDir = null;
                    //expandedDir = (item.Tag as CloudPageBlob);
                    //item.Items.Add(blockBlob.Name);
                }

               else if (item.Tag is CloudPageBlob)
                {
                    CloudPageBlob pageBlob = item.Tag as CloudPageBlob;
                    _BlobCollection.Add(new BlobItem()
                    {
                        Name = pageBlob.Name,
                        BlobType = "Page",
                        ContentType = pageBlob.Properties.ContentType,
                        Encoding = pageBlob.Properties.ContentEncoding,
                        Length = pageBlob.Properties.Length,
                        ETag = pageBlob.Properties.ETag,
                        LastModified = pageBlob.Properties.LastModified.Value.DateTime,
                        LastModifiedText = pageBlob.Properties.LastModified.Value.ToString()
                    });
                    //item.Items.Add(pageBlob.Name);
                }

                else if (item.Tag is CloudBlobDirectory)
                {
                    CloudBlobDirectory expandDir = null;
                    expandDir = (item.Tag as CloudBlobDirectory);
                    foreach (var subDir in expandDir.ListBlobs())
                        item.Items.Add(CreateTreeItem(subDir));
                }

                else
                {
                    CloudBlobContainer expandDir = null;
                    expandDir = (item.Tag as CloudBlobContainer);
                    foreach (var subDir in expandDir.ListBlobs())
                        item.Items.Add(CreateTreeItem(subDir));
                }

            }
        }


        private TreeViewItem CreateTreeItem(object o)
        {
            TreeViewItem item = new TreeViewItem();

            if (o.GetType() == typeof(CloudBlockBlob))
                item.Header = ((CloudBlockBlob)o).Name;

            else if (o.GetType() == typeof(CloudPageBlob))
            {
                item.Header = ((CloudPageBlob)o).Name;
            }

            else if (o.GetType() == typeof(CloudBlobDirectory))
            {
                item.Header = ((CloudBlobDirectory)o).Prefix.TrimEnd('/');
            }

            else
                item.Header = ((CloudBlobContainer)o).Name;

            item.Tag = o;
            item.Items.Add("Loading...");
            return item;
        }
    }
}
