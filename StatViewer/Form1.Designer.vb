<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.timerCheckConnections = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.SetupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IncreaseFontSizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DecreaseFontSizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MaximiseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.QuitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DownToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LeftToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RightToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Page1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Page2ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Page3ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Page4ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Page5ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Page6ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Page7ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Page8ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Page9ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Page10ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WebBrowser1.CausesValidation = False
        Me.WebBrowser1.Location = New System.Drawing.Point(0, 0)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.ScrollBarsEnabled = False
        Me.WebBrowser1.Size = New System.Drawing.Size(984, 3000)
        Me.WebBrowser1.TabIndex = 0
        Me.WebBrowser1.Url = New System.Uri("", System.UriKind.Relative)
        '
        'timerCheckConnections
        '
        Me.timerCheckConnections.Interval = 10000
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.Color.Black
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SetupToolStripMenuItem, Me.PageToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(984, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'SetupToolStripMenuItem
        '
        Me.SetupToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.IncreaseFontSizeToolStripMenuItem, Me.DecreaseFontSizeToolStripMenuItem, Me.MaximiseToolStripMenuItem, Me.ResetToolStripMenuItem, Me.QuitToolStripMenuItem, Me.MoveToolStripMenuItem})
        Me.SetupToolStripMenuItem.ForeColor = System.Drawing.Color.White
        Me.SetupToolStripMenuItem.Name = "SetupToolStripMenuItem"
        Me.SetupToolStripMenuItem.Size = New System.Drawing.Size(49, 20)
        Me.SetupToolStripMenuItem.Text = "Setup"
        '
        'IncreaseFontSizeToolStripMenuItem
        '
        Me.IncreaseFontSizeToolStripMenuItem.Name = "IncreaseFontSizeToolStripMenuItem"
        Me.IncreaseFontSizeToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Oemplus), System.Windows.Forms.Keys)
        Me.IncreaseFontSizeToolStripMenuItem.Size = New System.Drawing.Size(264, 22)
        Me.IncreaseFontSizeToolStripMenuItem.Text = "Increase Font Size"
        '
        'DecreaseFontSizeToolStripMenuItem
        '
        Me.DecreaseFontSizeToolStripMenuItem.Name = "DecreaseFontSizeToolStripMenuItem"
        Me.DecreaseFontSizeToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.OemMinus), System.Windows.Forms.Keys)
        Me.DecreaseFontSizeToolStripMenuItem.Size = New System.Drawing.Size(264, 22)
        Me.DecreaseFontSizeToolStripMenuItem.Text = "Decrease Font Size"
        '
        'MaximiseToolStripMenuItem
        '
        Me.MaximiseToolStripMenuItem.Name = "MaximiseToolStripMenuItem"
        Me.MaximiseToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.MaximiseToolStripMenuItem.Size = New System.Drawing.Size(264, 22)
        Me.MaximiseToolStripMenuItem.Text = "Maximise"
        '
        'ResetToolStripMenuItem
        '
        Me.ResetToolStripMenuItem.Name = "ResetToolStripMenuItem"
        Me.ResetToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.ResetToolStripMenuItem.Size = New System.Drawing.Size(264, 22)
        Me.ResetToolStripMenuItem.Text = "Reset"
        '
        'QuitToolStripMenuItem
        '
        Me.QuitToolStripMenuItem.Name = "QuitToolStripMenuItem"
        Me.QuitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Q), System.Windows.Forms.Keys)
        Me.QuitToolStripMenuItem.Size = New System.Drawing.Size(264, 22)
        Me.QuitToolStripMenuItem.Text = "Quit"
        '
        'MoveToolStripMenuItem
        '
        Me.MoveToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UpToolStripMenuItem, Me.DownToolStripMenuItem, Me.LeftToolStripMenuItem, Me.RightToolStripMenuItem})
        Me.MoveToolStripMenuItem.Name = "MoveToolStripMenuItem"
        Me.MoveToolStripMenuItem.Size = New System.Drawing.Size(264, 22)
        Me.MoveToolStripMenuItem.Text = "Move"
        '
        'UpToolStripMenuItem
        '
        Me.UpToolStripMenuItem.Name = "UpToolStripMenuItem"
        Me.UpToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Up), System.Windows.Forms.Keys)
        Me.UpToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.UpToolStripMenuItem.Text = "Up"
        '
        'DownToolStripMenuItem
        '
        Me.DownToolStripMenuItem.Name = "DownToolStripMenuItem"
        Me.DownToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Down), System.Windows.Forms.Keys)
        Me.DownToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.DownToolStripMenuItem.Text = "Down"
        '
        'LeftToolStripMenuItem
        '
        Me.LeftToolStripMenuItem.Name = "LeftToolStripMenuItem"
        Me.LeftToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Left), System.Windows.Forms.Keys)
        Me.LeftToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.LeftToolStripMenuItem.Text = "Left"
        '
        'RightToolStripMenuItem
        '
        Me.RightToolStripMenuItem.Name = "RightToolStripMenuItem"
        Me.RightToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Right), System.Windows.Forms.Keys)
        Me.RightToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.RightToolStripMenuItem.Text = "Right"
        '
        'PageToolStripMenuItem
        '
        Me.PageToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Page1ToolStripMenuItem, Me.Page2ToolStripMenuItem, Me.Page3ToolStripMenuItem, Me.Page4ToolStripMenuItem, Me.Page5ToolStripMenuItem, Me.Page6ToolStripMenuItem, Me.Page7ToolStripMenuItem, Me.Page8ToolStripMenuItem, Me.Page9ToolStripMenuItem, Me.Page10ToolStripMenuItem})
        Me.PageToolStripMenuItem.ForeColor = System.Drawing.Color.White
        Me.PageToolStripMenuItem.Name = "PageToolStripMenuItem"
        Me.PageToolStripMenuItem.Size = New System.Drawing.Size(45, 20)
        Me.PageToolStripMenuItem.Text = "Page"
        '
        'Page1ToolStripMenuItem
        '
        Me.Page1ToolStripMenuItem.Name = "Page1ToolStripMenuItem"
        Me.Page1ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D1), System.Windows.Forms.Keys)
        Me.Page1ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Page1ToolStripMenuItem.Text = "Page 1"
        '
        'Page2ToolStripMenuItem
        '
        Me.Page2ToolStripMenuItem.Name = "Page2ToolStripMenuItem"
        Me.Page2ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D2), System.Windows.Forms.Keys)
        Me.Page2ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Page2ToolStripMenuItem.Text = "Page 2"
        '
        'Page3ToolStripMenuItem
        '
        Me.Page3ToolStripMenuItem.Name = "Page3ToolStripMenuItem"
        Me.Page3ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D3), System.Windows.Forms.Keys)
        Me.Page3ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Page3ToolStripMenuItem.Text = "Page 3"
        '
        'Page4ToolStripMenuItem
        '
        Me.Page4ToolStripMenuItem.Name = "Page4ToolStripMenuItem"
        Me.Page4ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D4), System.Windows.Forms.Keys)
        Me.Page4ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Page4ToolStripMenuItem.Text = "Page 4"
        '
        'Page5ToolStripMenuItem
        '
        Me.Page5ToolStripMenuItem.Name = "Page5ToolStripMenuItem"
        Me.Page5ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D5), System.Windows.Forms.Keys)
        Me.Page5ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Page5ToolStripMenuItem.Text = "Page 5"
        '
        'Page6ToolStripMenuItem
        '
        Me.Page6ToolStripMenuItem.Name = "Page6ToolStripMenuItem"
        Me.Page6ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D6), System.Windows.Forms.Keys)
        Me.Page6ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Page6ToolStripMenuItem.Text = "Page 6"
        '
        'Page7ToolStripMenuItem
        '
        Me.Page7ToolStripMenuItem.Name = "Page7ToolStripMenuItem"
        Me.Page7ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D7), System.Windows.Forms.Keys)
        Me.Page7ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Page7ToolStripMenuItem.Text = "Page 7"
        '
        'Page8ToolStripMenuItem
        '
        Me.Page8ToolStripMenuItem.Name = "Page8ToolStripMenuItem"
        Me.Page8ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D8), System.Windows.Forms.Keys)
        Me.Page8ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Page8ToolStripMenuItem.Text = "Page 8"
        '
        'Page9ToolStripMenuItem
        '
        Me.Page9ToolStripMenuItem.Name = "Page9ToolStripMenuItem"
        Me.Page9ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D9), System.Windows.Forms.Keys)
        Me.Page9ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Page9ToolStripMenuItem.Text = "Page 9"
        '
        'Page10ToolStripMenuItem
        '
        Me.Page10ToolStripMenuItem.Name = "Page10ToolStripMenuItem"
        Me.Page10ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D0), System.Windows.Forms.Keys)
        Me.Page10ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Page10ToolStripMenuItem.Text = "Refresh"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(984, 561)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.WebBrowser1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Piranha StatViewer"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents timerCheckConnections As System.Windows.Forms.Timer
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents SetupToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IncreaseFontSizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DecreaseFontSizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MaximiseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ResetToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Page1ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Page2ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Page3ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Page4ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Page5ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Page6ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Page7ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Page8ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Page9ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Page10ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
    Friend WithEvents QuitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MoveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DownToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LeftToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RightToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
