class Programmm
{
    static void Main(string[] args)
    {
        RobotFighter robotA = new RobotFighter("Robot A", 100, 50, 20);
        bosRobot bos = new bosRobot("Bos Robot", 125, 60, 25, 15);

        Repair repair = new Repair();
        ElectricShock electricShock = new ElectricShock();
        PlasmaCannon plasmaCannon = new PlasmaCannon();
        SuperShield superShield = new SuperShield();

        while (robotA.IsAlive() && bos.IsAlive())
        {            
            robotA.CetakInformasi();
            bos.CetakInformasi();
            
            Console.WriteLine("Pilih aksi untuk Robot A:");
            Console.WriteLine("1. Serang");
            Console.WriteLine("2. Gunakan kemampuan");
            Console.Write("Pilihan: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    robotA.Serang(bos);
                    break;
                case "2":
                    Console.WriteLine($"\nPilih kemampuan yang ingin digunakan:");
                    Console.WriteLine("1. Gunakan Repair");
                    Console.WriteLine("2. Gunakan Electric Shock");
                    Console.WriteLine("3. Gunakan Plasma Cannon");
                    Console.WriteLine("4. Gunakan Super Shield");
                    Console.Write("Pilihan: ");
                    string kemampuanInput = Console.ReadLine();
                    switch (kemampuanInput)
                    {
                        case "1":
                            repair.Gunakan(robotA);
                            break;
                        case "2":
                            electricShock.Gunakan(bos);
                            break;
                        case "3":
                            plasmaCannon.Gunakan(bos);
                            break;
                        case "4":
                            superShield.Gunakan(robotA);
                            break;
                        default:
                            Console.WriteLine("Pilihan tidak valid.");
                            continue;
                    }
                    break;

                default:
                    Console.WriteLine("Pilihan tidak valid.");
                    continue;
            }
            repair.CooldownSelesai();
            electricShock.CooldownSelesai();
            plasmaCannon.CooldownSelesai();
            superShield.CooldownSelesai();
            if (bos.IsAlive())
            {
                bos.Serang(robotA);
            }
            if (!robotA.IsAlive())
            {
                robotA.isMati();
            }
            if (!bos.IsAlive())
            {
                bos.isMati();
            }
        }
    }
}

public abstract class Robot
{
    public string nama;
    public int energi, armor, serangan;

    public Robot(string nama, int energi, int armor, int serangan)
    {
        this.nama = nama;
        this.energi = energi;
        this.armor = armor;
        this.serangan = serangan;
    }

    public bool IsAlive()
    {
        return energi > 0;
    }

    public void Serang(Robot target)
    {
        Console.WriteLine($"{nama} menyerang {target.nama}!\n");

        if (target.armor > 0)
        {
            int sisaArmor = target.armor - this.serangan;
            if (sisaArmor >= 0)
            {
                target.armor = sisaArmor;
                Console.WriteLine($"{target.nama} berhasil menahan serangan! Sisa armor: {target.armor}\n");                
            }

            else
            {
                target.armor = 0;
                target.energi += sisaArmor;
                Console.WriteLine($"Armor {target.nama} hancur! Sisa energi: {target.energi}\n");
            }
        }
        else
        {
            target.armor = 0;            
            target.energi -= serangan;
            Console.WriteLine($"{target.nama} terkena serangan langsung! Sisa energi: {target.energi}\n");
        }
    }

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama: {nama}\nEnergi: {energi}\nArmor: {armor}\nSerangan: {serangan}\n");
    }

}

public interface IKemampuan
{
    void Gunakan(Robot target);
    int CooldownSelesai();
}

public class bosRobot : Robot
{
    public int pertahanan;

    public bosRobot(string nama, int energi, int armor, int serangan, int pertahanan) : base (nama, energi, armor, serangan)
    {
        this.armor += pertahanan;
    }

    public void Diserang(Robot penyerang)
    {
        Console.WriteLine($"{penyerang.nama} menyerang {nama}!");

        int totalPertahanan = armor + pertahanan;
        int damage = Math.Max(0, penyerang.serangan - totalPertahanan);

        if (damage > 0)
        {
            energi -= damage;
            Console.WriteLine($"{nama} menerima serangan! Energi berkurang {damage}. Sisa energi: {energi}");
        }
        else
        {
            Console.WriteLine($"{nama} berhasil menahan serangan sepenuhnya!");
        }

        if (!IsAlive())
        {
            Mati();
        }
    }  

    public void isMati()
    {
        if (!IsAlive())
        {
            Mati();
        }
    }

    public void Mati()
    {
        Console.WriteLine($"{nama} telah dikalahkan!\n");
    }
}

public class RobotFighter : Robot
{
    private IKemampuan kemampuan;
    public RobotFighter(string nama, int energi, int armor, int serangan) : base(nama, energi, armor, serangan)
    {

    }
    public void AkhiriGiliran()
    {
        kemampuan.CooldownSelesai();
    }

    public void isMati()
    {
        if (!IsAlive())
        {
            Mati();
        }
    }

    public void Mati()
    {
        Console.WriteLine($"{nama} telah dikalahkan!\n");
    }

    public void AkhiriGiliran(Repair repair, ElectricShock electricShock, PlasmaCannon plasmaCannon, SuperShield superShield)
    {
        repair.CooldownSelesai();
        electricShock.CooldownSelesai();
        plasmaCannon.CooldownSelesai();
        superShield.CooldownSelesai();
    }
}

public class Repair : IKemampuan
{
    private int cooldown = 3;
    private int giliranTersisa = 0;
    public string Nama = "Repair";

    public void Gunakan(Robot target)
    {
        if (giliranTersisa == 0)
        {
            target.energi += 30;
            Console.WriteLine($"{target.nama} memulihkan 30 energi!\n");
            giliranTersisa = cooldown;
        }

        else
        {
            Console.WriteLine($"Kemampuan Repair dalam cooldown! ({giliranTersisa} giliran lagi)\n");
        }
    }

    public int CooldownSelesai()
    {
        if (giliranTersisa > 0)
        { 
            giliranTersisa--;
        }
        return giliranTersisa;
    }
}

public class ElectricShock : IKemampuan
{
    private int cooldown = 2;
    private int giliranTersisa = 0;
    public string Nama = "Electric Shock";

    public void Gunakan(Robot target)
    {
        if (giliranTersisa == 0)
        {
            target.energi -= 20;
            Console.WriteLine($"{target.nama} terkena Electric Shock! Energi berkurang 20.\n");
            giliranTersisa = cooldown;
        }

        else
        {
            Console.WriteLine($"Kemampuan Electric Shock dalam cooldown! ({giliranTersisa} giliran lagi)\n");
        }
    }

    public int CooldownSelesai()
    {
        if (giliranTersisa > 0)
        {
            giliranTersisa--;
        }
        return giliranTersisa;
    }
}

public class PlasmaCannon : IKemampuan
{
    private int cooldown = 4;
    private int giliranTersisa = 0;
    public string Nama = "Plasma Cannon";

    public void Gunakan(Robot target)
    {
        if (giliranTersisa == 0)
        {
            int damage = 30;
            target.energi -= damage;
            Console.WriteLine($"{target.nama} terkena Tembakan Plasma! Energi berkurang 30.\n");
            giliranTersisa = cooldown;
        }

        else
        {
            Console.WriteLine($"Tembakan Plasma dalam cooldown! ({giliranTersisa} giliran lagi)\n");
        }
    }

    public int CooldownSelesai()
    {
        if (giliranTersisa > 0)
        {
            giliranTersisa--;
        }
        return giliranTersisa;
    }
}

public class SuperShield : IKemampuan
{
    private int cooldown = 5;
    private int giliranTersisa = 0;
    private int peningkatanArmor = 20;
    private int durasi = 2;
    private int durasiAktif = 0;
    public string Nama = "Super Shield";

    public void Gunakan(Robot target)
    {
        if (giliranTersisa == 0)
        {
            Console.WriteLine($"{target.nama} menggunakan Super Shield! Armor meningkat sebesar {peningkatanArmor} selama {durasi} giliran.\n");
            target.armor += peningkatanArmor;
            durasiAktif = durasi;
            giliranTersisa = cooldown;
        }
        else
        {
            Console.WriteLine($"Super Shield dalam cooldown! ({giliranTersisa} giliran lagi)\n");
        }
    }

    public int CooldownSelesai()
    {
        if (giliranTersisa > 0)
        {
            giliranTersisa--;
        }

        if (durasiAktif > 0)
        {
            durasiAktif--;
        }

        return giliranTersisa;
    }
}