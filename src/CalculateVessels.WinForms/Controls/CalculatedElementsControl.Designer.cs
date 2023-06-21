namespace CalculateVessels.Controls
{
    partial class CalculatedElementsControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculatedElementsControl));
            editButton = new System.Windows.Forms.Button();
            deleteButton = new System.Windows.Forms.Button();
            downButton = new System.Windows.Forms.Button();
            upButton = new System.Windows.Forms.Button();
            elementsListView = new System.Windows.Forms.ListView();
            SuspendLayout();
            // 
            // editButton
            // 
            editButton.Enabled = false;
            editButton.Location = new System.Drawing.Point(327, 140);
            editButton.Name = "editButton";
            editButton.Size = new System.Drawing.Size(40, 27);
            editButton.TabIndex = 51;
            editButton.Text = "Edit";
            editButton.UseVisualStyleBackColor = true;
            editButton.Click += EditButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.Enabled = false;
            deleteButton.Image = (System.Drawing.Image)resources.GetObject("deleteButton.Image");
            deleteButton.Location = new System.Drawing.Point(327, 103);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new System.Drawing.Size(40, 25);
            deleteButton.TabIndex = 50;
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += DeleteButton_Click;
            // 
            // downButton
            // 
            downButton.Enabled = false;
            downButton.Image = (System.Drawing.Image)resources.GetObject("downButton.Image");
            downButton.Location = new System.Drawing.Point(327, 52);
            downButton.Name = "downButton";
            downButton.Size = new System.Drawing.Size(40, 25);
            downButton.TabIndex = 49;
            downButton.UseVisualStyleBackColor = true;
            downButton.Click += DownButton_Click;
            // 
            // upButton
            // 
            upButton.Enabled = false;
            upButton.Image = (System.Drawing.Image)resources.GetObject("upButton.Image");
            upButton.Location = new System.Drawing.Point(327, 20);
            upButton.Name = "upButton";
            upButton.Size = new System.Drawing.Size(40, 25);
            upButton.TabIndex = 48;
            upButton.UseVisualStyleBackColor = true;
            upButton.Click += UpButton_Click;
            // 
            // elementsListView
            // 
            elementsListView.GridLines = true;
            elementsListView.Location = new System.Drawing.Point(0, 0);
            elementsListView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            elementsListView.MultiSelect = false;
            elementsListView.Name = "elementsListView";
            elementsListView.Size = new System.Drawing.Size(320, 380);
            elementsListView.TabIndex = 47;
            elementsListView.UseCompatibleStateImageBehavior = false;
            elementsListView.View = System.Windows.Forms.View.List;
            elementsListView.ItemSelectionChanged += ElementsListView_ItemSelectionChanged;
            // 
            // CalculatedElementsControl
            // 
            Controls.Add(editButton);
            Controls.Add(deleteButton);
            Controls.Add(downButton);
            Controls.Add(upButton);
            Controls.Add(elementsListView);
            Name = "CalculatedElementsControl";
            Size = new System.Drawing.Size(380, 380);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        internal System.Windows.Forms.ListView elementsListView;
    }
}
