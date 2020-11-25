using osu_LiveHelper.Util;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace osu_LiveHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool ACTION_LOCK = false;
        private CultureInfo CultureInfoCN = new CultureInfo("zh-Hans", false);
        private const string CANCEL_TOPMOST = "取消直播端游戏窗口置顶";
        private const string MERGE_LIVE_WINDOWS = "将直播端合并为单窗口";
        private const string MORE_ACTIONS = "更多功能";

        public MainWindow()
        {
            CultureInfo.CurrentCulture = CultureInfoCN;
            CultureInfo.CurrentUICulture = CultureInfoCN;
            InitializeComponent();
        }

        private void WindowClose(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Init(object sender, EventArgs e)
        {
            ActionComboBox.IsEnabled = false;
            ConfirmButton.IsEnabled = false;
            ResetButton.IsEnabled = false;

            Console.SetOut(new TextBoxStreamWriter(ConsoleTextBox));

            Console.WriteLine("初始化完成");
            Console.WriteLine("取消置顶功能说明：必须在所有 osu! 游戏端启动完成后使用，否则直播端将无法启动全部游戏端");
            Console.WriteLine("合并窗口功能说明：若同时需要取消置顶，必须先执行取消置顶，执行合并窗口后将无法执行取消置顶");
            Console.WriteLine("注意：点击执行后请等待下方窗口显示“执行完毕”后再退出本程序，否则可能导致无法预期的问题");
            Console.WriteLine("提示：游戏端全部启动完成后，可手动重启单个游戏端，之后再次执行需要的功能即可");

            ActionComboBox.IsEnabled = true;
            ConfirmButton.IsEnabled = true;
            ResetButton.IsEnabled = true;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            if (ACTION_LOCK)
            {
                Console.WriteLine("正在执行操作，请稍候...");
            }
            else
            {
                Environment.Exit(0);
            }
        }

        public void DoAction(object sender, EventArgs e)
        {
            switch (ActionComboBox.SelectedValue.ToString())
            {
                case CANCEL_TOPMOST:
                    Console.WriteLine("正在执行：" + CANCEL_TOPMOST);
                    ACTION_LOCK = true;
                    TournamentModifyUtil.CancelGameWindowsTopMost();
                    ACTION_LOCK = false;
                    Console.WriteLine("执行完毕");
                    break;
                case MERGE_LIVE_WINDOWS:
                    Console.WriteLine("正在执行：" + MERGE_LIVE_WINDOWS);
                    ACTION_LOCK = true;
                    TournamentModifyUtil.MergeLiveWindows();
                    ACTION_LOCK = false;
                    Console.WriteLine("执行完毕");
                    break;
                case MORE_ACTIONS:
                    Console.WriteLine("请提交 Issue 或 PR 请求来添加更多功能 :)");
                    break;
                default:
                    Console.WriteLine("无效操作");
                    break;
            }

        }

    }
}
