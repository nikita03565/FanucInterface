using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewEditModeTest {
    FanucModel model = new FanucModel();

    float[][] coords = new float[][]
     {
     new float[] { 40f, 40f, 40f, 40f, 40f, 40f },
     new float[] { 40f, 40f, 40f, 40f, 40f, -40f },
     new float[] { 40f, 40f, 40f, 40f, -40f, 40f },
     new float[] { 40f, 40f, 40f, 40f, 40f, -40f },
     new float[] { 40f, 40f, 40f, -40f, 40f, 40f },
     new float[] { 40f, 40f, 40f, -40f, 40f, -40f },
     new float[] { 40f, 40f, 40f, -40f, -40f, 40f },
     new float[] { 40f, 40f, 40f, -40f, -40f, -40f },
    
     new float[] { 40f, 40f, -40f, 40f, 40f, 40f },
     new float[] { 40f, 40f, -40f, 40f, 40f, -40f },
     new float[] { 40f, 40f, -40f, 40f, -40f, 40f },
     new float[] { 40f, 40f, -40f, 40f, 40f, -40f },
     new float[] { 40f, 40f, -40f, -40f, 40f, 40f },
     new float[] { 40f, 40f, -40f, -40f, 40f, -40f },
     new float[] { 40f, 40f, -40f, -40f, -40f, 40f },
     new float[] { 40f, 40f, -40f, -40f, -40f, -40f },
    
     new float[] { 40f, -40f, 40f, 40f, 40f, 40f },
     new float[] { 40f, -40f, 40f, 40f, 40f, -40f },
     new float[] { 40f, -40f, 40f, 40f, -40f, 40f },
     new float[] { 40f, -40f, 40f, 40f, 40f, -40f },
     new float[] { 40f, -40f, 40f, -40f, 40f, 40f },
     new float[] { 40f, -40f, 40f, -40f, 40f, -40f },
     new float[] { 40f, -40f, 40f, -40f, -40f, 40f },
     new float[] { 40f, -40f, 40f, -40f, -40f, -40f },
     
     new float[] { 40f, -40f, -40f, 40f, 40f, 40f },
     new float[] { 40f, -40f, -40f, 40f, 40f, -40f },
     new float[] { 40f, -40f, -40f, 40f, -40f, 40f },
     new float[] { 40f, -40f, -40f, 40f, 40f, -40f },
     new float[] { 40f, -40f, -40f, -40f, 40f, 40f },
     new float[] { 40f, -40f, -40f, -40f, 40f, -40f },
     new float[] { 40f, -40f, -40f, -40f, -40f, 40f },
     new float[] { 40f, -40f, -40f, -40f, -40f, -40f },
    
     new float[] { -40f, 40f, 40f, 40f, 40f, 40f },
     new float[] { -40f, 40f, 40f, 40f, 40f, -40f },
     new float[] { -40f, 40f, 40f, 40f, -40f, 40f },
     new float[] { -40f, 40f, 40f, 40f, 40f, -40f },
     new float[] { -40f, 40f, 40f, -40f, 40f, 40f },
     new float[] { -40f, 40f, 40f, -40f, 40f, -40f },
     new float[] { -40f, 40f, 40f, -40f, -40f, 40f },
     new float[] { -40f, 40f, 40f, -40f, -40f, -40f },
     
     new float[] { -40f, 40f, -40f, 40f, 40f, 40f },
     new float[] { -40f, 40f, -40f, 40f, 40f, -40f },
     new float[] { -40f, 40f, -40f, 40f, -40f, 40f },
     new float[] { -40f, 40f, -40f, 40f, 40f, -40f },
     new float[] { -40f, 40f, -40f, -40f, 40f, 40f },
     new float[] { -40f, 40f, -40f, -40f, 40f, -40f },
     new float[] { -40f, 40f, -40f, -40f, -40f, 40f },
     new float[] { -40f, 40f, -40f, -40f, -40f, -40f },
    
     new float[] { -40f, -40f, 40f, 40f, 40f, 40f },
     new float[] { -40f, -40f, 40f, 40f, 40f, -40f },
     new float[] { -40f, -40f, 40f, 40f, -40f, 40f },
     new float[] { -40f, -40f, 40f, 40f, 40f, -40f },
     new float[] { -40f, -40f, 40f, -40f, 40f, 40f },
     new float[] { -40f, -40f, 40f, -40f, 40f, -40f },
     new float[] { -40f, -40f, 40f, -40f, -40f, 40f },
     new float[] { -40f, -40f, 40f, -40f, -40f, -40f },
     
     new float[] { -40f, -40f, -40f, 40f, 40f, 40f },
     new float[] { -40f, -40f, -40f, 40f, 40f, -40f },
     new float[] { -40f, -40f, -40f, 40f, -40f, 40f },
     new float[] { -40f, -40f, -40f, 40f, 40f, -40f },
     new float[] { -40f, -40f, -40f, -40f, 40f, 40f },
     new float[] { -40f, -40f, -40f, -40f, 40f, -40f },
     new float[] { -40f, -40f, -40f, -40f, -40f, 40f },
     new float[] { -40f, -40f, -40f, -40f, -40f, -40f },
     
     new float[] { -40f, -40f, -40f, 140f, 40f, 140f },
     new float[] { -40f, -40f, -40f, 140f, 40f, -140f },
     new float[] { -40f, -40f, -40f, 140f, -40f, 140f },
     new float[] { -40f, -40f, -40f, 140f, 40f, -140f },
     new float[] { -40f, -40f, -40f, -140f, 40f, 140f },
     new float[] { -40f, -40f, -40f, -140f, 40f, -140f },
     new float[] { -40f, -40f, -40f, -140f, -40f, 140f },
     new float[] { -40f, -40f, -40f, -140f, -40f, -140f },
    
     new float[] { -40f, -40f, 0f, 0f, 40f, 140f },
     new float[] { -40f, -0f, -40f, 0f, 40f, -140f },
     new float[] { -40f, -40f, -40f, 0f, -40f, 140f },
     new float[] { 0f, -39f, -40f, 0f, 40f, -140f },//75
     new float[] { -40f, -40f, -39f, 0f, 40f, 0f },//76
     new float[] { -40f, 50f, -60f, 10f, 40f, -14 },
     new float[] { 40f, -40f, -40f, -10f, -40f, 140f },
     new float[] { 0.0f, -29.81f, -21.51f, 0.0f, -68.49f, 0.0f },
     new float[] { -40f, 40f, -40f, -140f, 40f, -140f }};

    bool compare(int num)
    {
        var res = FanucModel.GetCoordsFromMat(model.fanucForwardTask(ref coords[num]));
        var resInv = model.InverseTask(ref res);
        bool[] f = new bool[resInv.Length/6];
        for (int i = 0; i < resInv.Length / 6; ++i)
        {
            f[i] = true;
            for (int j = 0; j < 6; ++j)
            {
                if (Mathf.Abs(resInv[i, j] - coords[num][j]) > 0.5)
                {
                    f[i] = false;
                }
            }
        }
        bool finalF = false;
        for (int i = 0; i < resInv.Length / 6; ++i)
        {
            finalF = f[i] || finalF;
        }
        return finalF;
    }

    [Test]
	public void Test0()
    {
        Assert.AreEqual(true, compare(0));
    }

    [Test]
    public void Test1()
    {
        Assert.AreEqual(true, compare(1));
    }

    [Test]
    public void Test2()
    {
        Assert.AreEqual(true, compare(2));
    }
    [Test]
    public void Test3()
    {
        Assert.AreEqual(true, compare(3));
    }

    [Test]
    public void Test4()
    {
        Assert.AreEqual(true, compare(4));
    }

    [Test]
    public void Test5()
    {
        Assert.AreEqual(true, compare(5));
    }

    [Test]
    public void Test6()
    {
        Assert.AreEqual(true, compare(6));
    }

    [Test]
    public void Test7()
    {
        Assert.AreEqual(true, compare(7));
    }
    [Test]
    public void Test8()
    {
        Assert.AreEqual(true, compare(8));
    }

    [Test]
    public void Test9()
    {
        Assert.AreEqual(true, compare(9));
    }

    [Test]
    public void Test10()
    {
        Assert.AreEqual(true, compare(10));
    }

    [Test]
    public void Test11()
    {
        Assert.AreEqual(true, compare(11));
    }

    [Test]
    public void Test12()
    {
        Assert.AreEqual(true, compare(12));
    }
    [Test]
    public void Test13()
    {
        Assert.AreEqual(true, compare(13));
    }

    [Test]
    public void Test14()
    {
        Assert.AreEqual(true, compare(14));
    }

    [Test]
    public void Test15()
    {
        Assert.AreEqual(true, compare(15));
    }

    [Test]
    public void Test16()
    {
        Assert.AreEqual(true, compare(16));
    }

    [Test]
    public void Test17()
    {
        Assert.AreEqual(true, compare(17));
    }
    [Test]
    public void Test18()
    {
        Assert.AreEqual(true, compare(18));
    }

    [Test]
    public void Test19()
    {
        Assert.AreEqual(true, compare(19));
    }

    [Test]
    public void Test20()
    {
        Assert.AreEqual(true, compare(20));
    }

    [Test]
    public void Test21()
    {
        Assert.AreEqual(true, compare(21));
    }

    [Test]
    public void Test22()
    {
        Assert.AreEqual(true, compare(22));
    }
    [Test]
    public void Test23()
    {
        Assert.AreEqual(true, compare(23));
    }

    [Test]
    public void Test24()
    {
        Assert.AreEqual(true, compare(24));
    }

    [Test]
    public void Test25()
    {
        Assert.AreEqual(true, compare(25));
    }

    [Test]
    public void Test26()
    {
        Assert.AreEqual(true, compare(26));
    }

    [Test]
    public void Test27()
    {
        Assert.AreEqual(true, compare(27));
    }
    [Test]
    public void Test28()
    {
        Assert.AreEqual(true, compare(28));
    }

    [Test]
    public void Test29()
    {
        Assert.AreEqual(true, compare(29));
    }

    [Test]
    public void Test30()
    {
        Assert.AreEqual(true, compare(30));
    }

    [Test]
    public void Test31()
    {
        Assert.AreEqual(true, compare(31));
    }

    [Test]
    public void Test32()
    {
        Assert.AreEqual(true, compare(32));
    }
    [Test]
    public void Test33()
    {
        Assert.AreEqual(true, compare(33));
    }

    [Test]
    public void Test34()
    {
        Assert.AreEqual(true, compare(34));
    }

    [Test]
    public void Test35()
    {
        Assert.AreEqual(true, compare(35));
    }

    [Test]
    public void Test36()
    {
        Assert.AreEqual(true, compare(36));
    }

    [Test]
    public void Test37()
    {
        Assert.AreEqual(true, compare(37));
    }
    [Test]
    public void Test38()
    {
        Assert.AreEqual(true, compare(38));
    }

    [Test]
    public void Test39()
    {
        Assert.AreEqual(true, compare(39));
    }

    [Test]
    public void Test40()
    {
        Assert.AreEqual(true, compare(40));
    }

    [Test]
    public void Test41()
    {
        Assert.AreEqual(true, compare(41));
    }

    [Test]
    public void Test42()
    {
        Assert.AreEqual(true, compare(42));
    }
    [Test]
    public void Test43()
    {
        Assert.AreEqual(true, compare(43));
    }

    [Test]
    public void Test44()
    {
        Assert.AreEqual(true, compare(44));
    }

    [Test]
    public void Test45()
    {
        Assert.AreEqual(true, compare(45));
    }

    [Test]
    public void Test46()
    {
        Assert.AreEqual(true, compare(46));
    }

    [Test]
    public void Test47()
    {
        Assert.AreEqual(true, compare(47));
    }
    [Test]
    public void Test48()
    {
        Assert.AreEqual(true, compare(48));
    }

    [Test]
    public void Test49()
    {
        Assert.AreEqual(true, compare(49));
    }

    [Test]
    public void Test50()
    {
        Assert.AreEqual(true, compare(50));
    }

    [Test]
    public void Test51()
    {
        Assert.AreEqual(true, compare(51));
    }

    [Test]
    public void Test52()
    {
        Assert.AreEqual(true, compare(52));
    }
    [Test]
    public void Test53()
    {
        Assert.AreEqual(true, compare(53));
    }

    [Test]
    public void Test54()
    {
        Assert.AreEqual(true, compare(54));
    }

    [Test]
    public void Test55()
    {
        Assert.AreEqual(true, compare(55));
    }

    [Test]
    public void Test56()
    {
        Assert.AreEqual(true, compare(56));
    }

    [Test]
    public void Test57()
    {
        Assert.AreEqual(true, compare(57));
    }
    [Test]
    public void Test58()
    {
        Assert.AreEqual(true, compare(58));
    }

    [Test]
    public void Test59()
    {
        Assert.AreEqual(true, compare(59));
    }

    [Test]
    public void Test60()
    {
        Assert.AreEqual(true, compare(60));
    }

    [Test]
    public void Test61()
    {
        Assert.AreEqual(true, compare(61));
    }

    [Test]
    public void Test62()
    {
        Assert.AreEqual(true, compare(62));
    }
    [Test]
    public void Test63()
    {
        Assert.AreEqual(true, compare(63));
    }

    [Test]
    public void Test64()
    {
        Assert.AreEqual(true, compare(64));
    }

    [Test]
    public void Test65()
    {
        Assert.AreEqual(true, compare(65));
    }

    [Test]
    public void Test66()
    {
        Assert.AreEqual(true, compare(66));
    }

    [Test]
    public void Test67()
    {
        Assert.AreEqual(true, compare(67));
    }
    [Test]
    public void Test68()
    {
        Assert.AreEqual(true, compare(68));
    }

    [Test]
    public void Test69()
    {
        Assert.AreEqual(true, compare(69));
    }

    [Test]
    public void Test70()
    {
        Assert.AreEqual(true, compare(70));
    }

    [Test]
    public void Test71()
    {
        Assert.AreEqual(true, compare(71));
    }

    [Test]
    public void Test72()
    {
        Assert.AreEqual(true, compare(72));
    }
    [Test]
    public void Test73()
    {
        Assert.AreEqual(true, compare(73));
    }

    [Test]
    public void Test74()
    {
        Assert.AreEqual(true, compare(74));
    }

    [Test]
    public void Test75()
    {
        Assert.AreEqual(true, compare(75));
    }

    [Test]
    public void Test76()
    {
        Assert.AreEqual(true, compare(76));
    }

    [Test]
    public void Test77()
    {
        Assert.AreEqual(true, compare(77));
    }
    [Test]
    public void Test78()
    {
        Assert.AreEqual(true, compare(78));
    }

    [Test]
    public void Test79()
    {
        Assert.AreEqual(true, compare(79));
    }

    [Test]
    public void Test80()
    {
        Assert.AreEqual(true, compare(80));
    }
}
