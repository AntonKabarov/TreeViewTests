using LinqToDB;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LinqToDB.Data;
using System.Linq;

namespace ClassLibrary2
{
    public struct CopyFileInfo
    {
        public string filename;
        public string filepath;
    }
    public class TreeViewClass
    {


        System.Windows.Forms.ListBox listBox1;
        System.Windows.Forms.TreeView treeView1;

        bool isCtrlPressed = false; // для Ctrl+C, Ctrl+V сочетания
        CopyFileInfo copiedFile; // сохраняет путь к файлу скопировано Ctrl+C 

        public DataConnection dataContext = new DataConnection(Connect.ProviderName, Connect.Connectbd);


        public System.Windows.Forms.ListBox ListBox1
        {
            get { return listBox1; }
            set { listBox1 = value; } 
        
        }

        public System.Windows.Forms.TreeView TreeView1
        {
            get { return treeView1; }
            set { treeView1 = value; }


        }
        public void TreeView1_AfterSelect()
        {
            listBox1.Items.Clear();
            FileListBuilder(treeView1.SelectedNode.FullPath);
        }

    
        public void listBox1_KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                if (e.KeyCode == Keys.ControlKey)
                    isCtrlPressed = true;

            if (isCtrlPressed && (e.KeyCode == Keys.C))
            {
                // Скопировать выбранный файл

                // Получить расположение файла
                string selected_filename = listBox1.SelectedItem.ToString();
                string current_path = treeView1.SelectedNode.FullPath;

                // хранить его путь и имя в переменной
                if (!String.IsNullOrEmpty(selected_filename))
                {
                    copiedFile.filepath = current_path;
                    copiedFile.filename = selected_filename;
                }

                isCtrlPressed = false;
            }

            if (isCtrlPressed && (e.KeyCode == Keys.V))
            {
                // Вставить скопированный файл

                // Получить текущий путь к каталогу
                string current_path = treeView1.SelectedNode.FullPath;

                // Скопировать ранее указал файл в текущем каталоге
                if (!String.IsNullOrEmpty(copiedFile.filename))
                {
                    File.Copy(
                        Path.Combine(copiedFile.filepath, copiedFile.filename),
                        Path.Combine(current_path, copiedFile.filename)
                    );
                    // Обновить список файлов
                    listBox1.Items.Clear();
                    FileListBuilder(treeView1.SelectedNode.FullPath);
                }

                isCtrlPressed = false;
            }

        }
        public void MouseClick()
        {
            if (this.listBox1.SelectedItem is not null)
                this.listBox1.DoDragDrop(this.listBox1.SelectedItem, DragDropEffects.Move);
        }
        public void treeView1_DragDrop(DragEventArgs e)
        {
            //GetNodeAt - Возвращает узел дерева, который находится в указанном расположении.
            TreeNode nodeToDropIn = treeView1.GetNodeAt(this.treeView1.PointToClient(new Point(e.X, e.Y)));
            if (nodeToDropIn == null) { return; }
            if (nodeToDropIn.Level < 0)
            {
                nodeToDropIn = nodeToDropIn.Parent;
            }
            // Адрес nodeToDropIn
            string target_path = nodeToDropIn.FullPath;
            if (!String.IsNullOrEmpty(copiedFile.filename))
            {
                // Копировать вытащенный файл в целевой каталог
                File.Copy(
                    Path.Combine(copiedFile.filepath, copiedFile.filename),
                    Path.Combine(target_path, copiedFile.filename)
                );

                (from u in dataContext.GetTable<Files>()
                 where u.NameFile.Contains(copiedFile.filename)
                 select u).Delete();

                // Удалить оригинальный файл в исходном пути
                File.Delete(Path.Combine(copiedFile.filepath, copiedFile.filename));

                // Обновление ListBox
                listBox1.Items.Clear();
                FileListBuilder(treeView1.SelectedNode.FullPath);
            }
        }

        public void listBox1_DragOver(DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            // сохранить вытащенное местоположение файла
            string selected_filename = listBox1.SelectedItem.ToString();
            string current_path = treeView1.SelectedNode.FullPath;

            if (!String.IsNullOrEmpty(selected_filename))
            {
                copiedFile.filepath = current_path;
                copiedFile.filename = selected_filename;
            }
        }





        public void treeView1_Before(TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            string[] dirs;
            try
            {
                if (Directory.Exists(e.Node.FullPath))
                {
                    dirs = Directory.GetDirectories(e.Node.FullPath);
                    if (dirs.Length != 0)
                    {
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            TreeNode dirNode = new TreeNode(new DirectoryInfo(dirs[i]).Name);
                            FillTreeNode(dirNode, dirs[i]);
                            e.Node.Nodes.Add(dirNode);
                        }
                    }
                }
            }

            catch (Exception ex) { }


        }


        public void treeView1_After(TreeViewEventArgs e)
        {
            e.Node.Nodes.Clear();
            string[] dirs;
            try
            {
                if (Directory.Exists(e.Node.FullPath))
                {
                    dirs = Directory.GetDirectories(e.Node.FullPath);
                    if (dirs.Length != 0)
                    {
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            TreeNode dirNode = new TreeNode(new DirectoryInfo(dirs[i]).Name);
                            FillTreeNode(dirNode, dirs[i]);
                            e.Node.Nodes.Add(dirNode);
                        }
                    }
                }
            }

            catch (Exception ex) { }


        }




        public void deletebtn()
        {
            string selected_filename = listBox1.SelectedItem.ToString();
            string current_path = treeView1.SelectedNode.FullPath;

            //delete
            (from u in dataContext.GetTable<Files>()
             where u.NameFile.Contains(selected_filename)
             select u).Delete();





            //dataContext.Delete<Files>(new Files() {NameFile=selected_filename });


            if (!String.IsNullOrEmpty(selected_filename))
                File.Delete(Path.Combine(current_path, selected_filename));

            // обновить список файлов
            listBox1.Items.Clear();
            FileListBuilder(treeView1.SelectedNode.FullPath);
        }



        public void FileListBuilder(string p)
        {
            try
            {
                //exists category id
                var querycategory = (from u in dataContext.GetTable<Catalog>()
                             where u.Name.Contains(p)
                             select u);
                var existscategory = querycategory.Any();
                int categoryid;

                if (existscategory)
                {
                    categoryid = (from u in dataContext.GetTable<Catalog>()
                                  where u.Name.Contains(p)
                                  select u.Id).First();


                }
                else
                {
                 categoryid = new Random().Next();
                 dataContext.Insert<Catalog>(new Catalog() { Id = categoryid, Name = p });
                }
                
                
                
                
                
                String[] fileList = System.IO.Directory.GetFiles(p);
                String fileName;
                System.Collections.IEnumerator myEnum = fileList.GetEnumerator();
                while (myEnum.MoveNext())
                {
                  

                    int idelement = new Random().Next();

                    fileName = myEnum.Current.ToString();
                    this.listBox1.Items.Add(System.IO.Path.GetFileName(fileName));

                    //exists file
                    var queryfilename = (
                                        from u in dataContext.GetTable<Files>()
                                        where u.NameFile.Contains(System.IO.Path.GetFileName(fileName))
                                        select u
                                        ).Any();


                    if(!queryfilename)
                    dataContext.Insert<Files>(new Files() { Id = idelement, NameFile = System.IO.Path.GetFileName(fileName), CatalogId = categoryid, PathFile = System.IO.Path.GetFullPath(fileName), Type = new FileInfo(fileName).Extension });


                }
            }
            catch (Exception ex)
            {

            }



        }

        // заполняем дерево дисками
        public void FillDriveNodes()
        {
            try
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    TreeNode driveNode = new TreeNode { Text = drive.Name };
                    FillTreeNode(driveNode, drive.Name);
                    treeView1.Nodes.Add(driveNode);

                }
            }
            catch (Exception ex) { }
        }

        // получаем дочерние узлы для определенного узла
        private void FillTreeNode(TreeNode driveNode, string path)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);
               
                foreach (string dir in dirs)
                {
                    int idelement = new Random().Next();
                    TreeNode dirNode = new TreeNode();
                    dirNode.Text = dir.Remove(0, dir.LastIndexOf("\\") + 1);
                    driveNode.Nodes.Add(dirNode);

                }
            }
            catch (Exception ex) { }
        }


    }
}
