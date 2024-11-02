using System;
using System.Windows.Forms;

namespace duaxe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            over.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        int carspeed = 3;
        int score;
        int collectedcoin = 0;
        Random pos = new Random();
        private Form2 form2; // Khai báo một biến để giữ thể hiện của Form2

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && mycar.Left > 0) mycar.Left -= 3;
            if (e.KeyCode == Keys.Right && mycar.Left < 400) mycar.Left += 3;
            if (e.KeyCode == Keys.Up && carspeed < 30) carspeed++;
            if (e.KeyCode == Keys.Down && carspeed > 3) carspeed--;
        }

        void totalscore()
        {
            textBox1.Text = score.ToString();

            // Kiểm tra nếu điểm số đạt 20
            if (score >= 5 && form2 == null)
            {
                this.Hide(); // Ẩn Form1
                form2 = new Form2(); // Tạo mới chỉ khi chưa có form2
                form2.Show(); // Hiển thị Form2
                timer1.Enabled = false;

                // Đăng ký sự kiện để reset form2
                form2.FormClosed += (s, args) =>
                {
                    form2 = null; // Đặt lại biến khi Form2 đóng
                    this.Show(); // Hiện lại Form1
                };
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            linemove(carspeed);
            gameover();
            totalscore(); // Gọi totalscore
            Coinscollection();
        }

        void linemove(int speed)
        {
            MoveLine(pictureBox1, speed);
            MoveLine(pictureBox2, speed);
            MoveLine(pictureBox3, speed);
            MoveEnemy(enemy1, speed);
            MoveEnemy(enemy2, speed);
            MoveEnemy(enemy3, speed);

            MoveCoin(v1, speed);
            MoveCoin(v2, speed);
            MoveCoin(v4, speed);
            MoveCoin(v5, speed);
        }

        void MoveLine(PictureBox line, int speed)
        {
            if (line.Top > 500) line.Top = 0;
            else line.Top += speed;
        }

        void MoveEnemy(PictureBox enemy, int speed)
        {
            if (enemy.Top > 500)
            {
                score++;
                enemy.Left = pos.Next(0, 400);
                enemy.Top = 0;
            }
            else enemy.Top += speed;
        }

        void MoveCoin(PictureBox coin, int speed)
        {
            if (coin.Top > 500)
            {
                coin.Left = pos.Next(0, 400);
                coin.Top = 0;
            }
            else coin.Top += speed;
        }

        void Coinscollection()
        {
            CheckCoinCollection(v1);
            CheckCoinCollection(v2);
            CheckCoinCollection(v4);
            CheckCoinCollection(v5);
        }

        int lives = 0; // Số lượt chơi ban đầu

        void CheckCoinCollection(PictureBox coin)
        {
            if (mycar.Bounds.IntersectsWith(coin.Bounds))
            {
                collectedcoin++;
                coins.Text = "V = " + collectedcoin.ToString();
                coin.Left = pos.Next(0, 400);
                coin.Top = pos.Next(0, 40);

                // Tăng tốc độ sau mỗi đồng xu
                if (collectedcoin == 2 || collectedcoin == 6)
                {
                    carspeed++;
                }

                // Thêm lượt chơi
                if (collectedcoin == 2 || collectedcoin == 4)
                {
                    lives++;
                    livesLabel.Text = "Lives: " + lives.ToString(); // Cập nhật hiển thị lượt chơi
                }
            }
        }

        void gameover()
        {
            if (mycar.Bounds.IntersectsWith(enemy1.Bounds) ||
                 mycar.Bounds.IntersectsWith(enemy2.Bounds) ||
                 mycar.Bounds.IntersectsWith(enemy3.Bounds))
            {
                lives--; // Giảm số lượt chơi khi va chạm với kẻ thù
                livesLabel.Text = "Lives: " + lives.ToString(); // Cập nhật hiển thị lượt chơi

                if (lives <= 0)
                {
                    timer1.Enabled = false;
                    DialogResult go = MessageBox.Show("Do you want to play again?", "GAME OVER", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    if (go == DialogResult.Yes)
                    {
                        ResetGame();
                    }
                    else
                    {
                        this.Close();
                    }
                    over.Visible = true;
                }
                else
                {
                    // Nếu vẫn còn lượt chơi, reset vị trí mà không reset game
                    ResetPositions();
                }
            }
        }

        void ResetGame()
        {
            timer1.Enabled = true;
            score = 0;
            collectedcoin = 0; // Đặt lại số đồng xu về 0
            coins.Text = "V = 0"; // Cập nhật giao diện hiển thị số đồng xu
            lives = 0; // Reset số lượt chơi về mặc định
            livesLabel.Text = "Lives: " + lives.ToString(); // Cập nhật hiển thị số lượt chơi
            carspeed = 3; // Reset tốc độ về mặc định
            ResetPositions();
        }

        void ResetPositions()
        {
            foreach (var enemy in new[] { enemy1, enemy2, enemy3 })
            {
                enemy.Left = pos.Next(0, 400);
                enemy.Top = 0;
            }

            foreach (var coin in new[] { v1, v2, v4, v5 })
            {
                coin.Left = pos.Next(0, 400);
                coin.Top = pos.Next(0, 40);
            }
        }
    }
}
