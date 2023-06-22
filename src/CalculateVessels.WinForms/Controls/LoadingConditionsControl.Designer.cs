namespace CalculateVessels.Controls;

partial class LoadingConditionsControl
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
        deleteLoadingCondition_btn = new System.Windows.Forms.Button();
        addLoadingCondition_btn = new System.Windows.Forms.Button();
        loadingConditionsListView = new System.Windows.Forms.ListView();
        pressureType_ch = new System.Windows.Forms.ColumnHeader();
        p_ch = new System.Windows.Forms.ColumnHeader();
        t_ch = new System.Windows.Forms.ColumnHeader();
        sigmaAllow_ch = new System.Windows.Forms.ColumnHeader();
        EAllow_ch = new System.Windows.Forms.ColumnHeader();
        ordinalNumber_ch = new System.Windows.Forms.ColumnHeader();
        SuspendLayout();
        // 
        // deleteLoadingCondition_btn
        // 
        deleteLoadingCondition_btn.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        deleteLoadingCondition_btn.Location = new System.Drawing.Point(349, 58);
        deleteLoadingCondition_btn.Name = "deleteLoadingCondition_btn";
        deleteLoadingCondition_btn.Size = new System.Drawing.Size(25, 50);
        deleteLoadingCondition_btn.TabIndex = 72;
        deleteLoadingCondition_btn.Text = "-";
        deleteLoadingCondition_btn.UseVisualStyleBackColor = true;
        deleteLoadingCondition_btn.Click += DeleteLoadingCondition_btn_Click;
        // 
        // addLoadingCondition_btn
        // 
        addLoadingCondition_btn.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        addLoadingCondition_btn.ForeColor = System.Drawing.Color.Red;
        addLoadingCondition_btn.Location = new System.Drawing.Point(349, 0);
        addLoadingCondition_btn.Name = "addLoadingCondition_btn";
        addLoadingCondition_btn.Size = new System.Drawing.Size(25, 50);
        addLoadingCondition_btn.TabIndex = 71;
        addLoadingCondition_btn.Text = "+";
        addLoadingCondition_btn.UseVisualStyleBackColor = true;
        addLoadingCondition_btn.Click += AddLoadingCondition_btn_Click;
        // 
        // loadingConditionsListView
        // 
        loadingConditionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { ordinalNumber_ch, pressureType_ch, p_ch, t_ch, sigmaAllow_ch, EAllow_ch });
        loadingConditionsListView.FullRowSelect = true;
        loadingConditionsListView.GridLines = true;
        loadingConditionsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
        loadingConditionsListView.Location = new System.Drawing.Point(3, 3);
        loadingConditionsListView.MultiSelect = false;
        loadingConditionsListView.Name = "loadingConditionsListView";
        loadingConditionsListView.Size = new System.Drawing.Size(340, 108);
        loadingConditionsListView.TabIndex = 70;
        loadingConditionsListView.UseCompatibleStateImageBehavior = false;
        loadingConditionsListView.View = System.Windows.Forms.View.Details;
        // 
        // pressureType_ch
        // 
        pressureType_ch.Text = "";
        pressureType_ch.Width = 75;
        // 
        // p_ch
        // 
        p_ch.Text = "p, МПа";
        p_ch.Width = 55;
        // 
        // t_ch
        // 
        t_ch.Text = "T, °C";
        t_ch.Width = 55;
        // 
        // sigmaAllow_ch
        // 
        sigmaAllow_ch.Text = "[σ], МПа";
        // 
        // EAllow_ch
        // 
        EAllow_ch.Text = "E, МПа";
        // 
        // ordinalNumber_ch
        // 
        ordinalNumber_ch.Text = "№";
        ordinalNumber_ch.Width = 30;
        // 
        // LoadingConditionsControl
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(deleteLoadingCondition_btn);
        Controls.Add(addLoadingCondition_btn);
        Controls.Add(loadingConditionsListView);
        Name = "LoadingConditionsControl";
        Size = new System.Drawing.Size(380, 115);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Button deleteLoadingCondition_btn;
    private System.Windows.Forms.Button addLoadingCondition_btn;
    private System.Windows.Forms.ListView loadingConditionsListView;
    private System.Windows.Forms.ColumnHeader pressureType_ch;
    private System.Windows.Forms.ColumnHeader p_ch;
    private System.Windows.Forms.ColumnHeader t_ch;
    private System.Windows.Forms.ColumnHeader sigmaAllow_ch;
    private System.Windows.Forms.ColumnHeader EAllow_ch;
    private System.Windows.Forms.ColumnHeader ordinalNumber_ch;
}