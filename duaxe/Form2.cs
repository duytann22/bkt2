using System;
using System.Drawing;
using System.Windows.Forms;

namespace duaxe
{
    public partial class Form2 : Form
    {
        int carspeed = 3;
        int score;
        int collectedcoin = 0;
        int lives = 1; // Số lượt chơi ban đầu
        Random pos = new Random();
        private object enemy3;

        public Form2()

        {
            InitializeComponent();
            over.Visible = false;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Thiết lập các biến ban đầu
            livesLabel.Text = "Lives: " + lives.ToString();
        }

        private void Form2_KeyDown_1(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Left && mycar.Left > 0) mycar.Left -= 3;
            if (e.KeyCode == Keys.Right && mycar.Left < 400) mycar.Left += 3;
            if (e.KeyCode == Keys.Up && carspeed < 30) carspeed++;
            if (e.KeyCode == Keys.Down && carspeed > 3) carspeed--;

        }


        private void timer1_Tick_1(object sender, EventArgs e)

        {
            linemove(carspeed);
            gameover();
            totalscore();
            Coinscollection();
        }



        void linemove(int speed)
        {
            MoveLine(pictureBox1, speed);
            MoveLine(pictureBox2, speed);
            MoveLine(pictureBox3, speed);
            MoveEnemy(enemy1, speed);
            MoveEnemy(enemy2, speed);
            MoveEnemy(enemy5, speed);
            MoveEnemy(enemy4, speed);
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

        void CheckCoinCollection(PictureBox coin)
        {
            if (mycar.Bounds.IntersectsWith(coin.Bounds))
            {
                collectedcoin++;
                coins.Text = "V = " + collectedcoin.ToString();
                coin.Left = pos.Next(0, 400);
                coin.Top = pos.Next(0, 40);

                // Tăng tốc độ sau mỗi đồng xu
                if (collectedcoin % 4 == 0)
                {
                    carspeed++;
                }

                // Thêm lượt chơi
                if (collectedcoin == 5 || collectedcoin == 13)
                {
                    lives++;
                    livesLabel.Text = "Lives: " + lives.ToString(); // Cập nhật hiển thị lượt chơi
                }
            }
        }

        bool isGameOver = false;

        void gameover()
        {
             // Nếu đã game over, không làm gì thêm

            if (mycar.Bounds.IntersectsWith(enemy1.Bounds) ||
                mycar.Bounds.IntersectsWith(enemy2.Bounds) ||
                mycar.Bounds.IntersectsWith(enemy4.Bounds) ||
                mycar.Bounds.IntersectsWith(enemy5.Bounds))
            {
                lives--;
                livesLabel.Text = "Lives: " + lives.ToString();

                if (lives <= 0)
                {
                    isGameOver = true; // Đánh dấu là game over
                    timer1.Enabled = false; // Dừng timer
                    DialogResult go = MessageBox.Show("Game Over! Do you want to play again?", "GAME OVER",MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                    if (go == DialogResult.Yes)
                    {
                        ResetGame(); // Khởi động lại trò chowi                
                    }
                    else
                    {
                        Application.Exit();// Đóng ứng dụng
                    }
                    over.Visible = false;
                }
                else
                {
                    ResetPositions(); // Reset vị trí kẻ thù
                }
            }
        }





        void totalscore()
        {
            textBox1.Text = score.ToString();

            // Kiểm tra nếu điểm số đạt 10
            if (score >= 10)
            {
                timer1.Enabled = false; // Dừng timer
               
                DialogResult go = MessageBox.Show("YOU WIN!", "Play again", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                if (go == DialogResult.Yes)
                {
                    // Nếu chọn Yes, chuyển về Form1
                    this.Hide(); // Ẩn form hiện tại
                    Form1 form1 = new Form1(); // Tạo một instance mới của Form1
                    form1.Show(); // Hiện Form1
                }
                else
                {
                    // Nếu chọn No, thoát ứng dụng
                    Application.Exit();
                }
                over.Visible = true;
            }
        }


        void ResetGame()
        {
            score = 0; // Đặt lại điểm số
            collectedcoin = 0; // Đặt lại số đồng xu về 0
            coins.Text = "V = 0"; // Cập nhật giao diện hiển thị số đồng xu
            lives = 0; // Đặt lại số lượt chơi về mặc định
            livesLabel.Text = "Lives: " + lives.ToString(); // Cập nhật hiển thị số lượt chơi
            carspeed = 3; // Reset tốc độ về mặc định
            timer1.Enabled = true; // Bắt đầu timer
            ResetPositions(); // Reset vị trí của kẻ thù và đồng xu
        }


        void ResetPositions()
        {
            foreach (var enemy in new[] { enemy1, enemy2, enemy5 })
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
