using UnityEngine;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    [SerializeField] Image l1;
    [SerializeField] Image l2;
    [SerializeField] Image l3;
    [SerializeField] Image l4;

    [SerializeField] Image b;

    [SerializeField] Image r1;
    [SerializeField] Image r2;
    [SerializeField] Image r3;
    [SerializeField] Image r4;

    [SerializeField] GameObject w1;
    [SerializeField] GameObject w2;
    [SerializeField] GameObject winScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winScreen.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("W1") && collision.CompareTag("Package"))
        {
            winScreen.SetActive(true);
        }

        //LEFTSIDE
        if (collision.CompareTag("L1"))
        {
            l1.color = Color.red;
            l2.color = Color.white;
            l3.color = Color.white;
            l4.color = Color.white;

            b.color = Color.white;

            r1.color = Color.white;
            r2.color = Color.white;
            r3.color = Color.white;
            r4.color = Color.white;
        }
        if (collision.CompareTag("L2"))
        {
            l1.color = Color.white;
            l2.color = Color.red;
            l3.color = Color.white;
            l4.color = Color.white;

            b.color = Color.white;

            r1.color = Color.white;
            r2.color = Color.white;
            r3.color = Color.white;
            r4.color = Color.white;
        }
        if (collision.CompareTag("L3"))
        {
            l1.color = Color.white;
            l2.color = Color.white;
            l3.color = Color.red;
            l4.color = Color.white;

            b.color = Color.white;

            r1.color = Color.white;
            r2.color = Color.white;
            r3.color = Color.white;
            r4.color = Color.white;
        }
        if (collision.CompareTag("L4"))
        {
            l1.color = Color.white;
            l2.color = Color.white;
            l3.color = Color.white;
            l4.color = Color.red;

            b.color = Color.white;

            r1.color = Color.white;
            r2.color = Color.white;
            r3.color = Color.white;
            r4.color = Color.white;
        }

        //RIGHTSIDE
        if (collision.CompareTag("R1"))
        {
            l1.color = Color.white;
            l2.color = Color.white;
            l3.color = Color.white;
            l4.color = Color.white;

            b.color = Color.white;

            r1.color = Color.blue;
            r2.color = Color.white;
            r3.color = Color.white;
            r4.color = Color.white;
        }
        if (collision.CompareTag("R2"))
        {
            l1.color = Color.white;
            l2.color = Color.white;
            l3.color = Color.white;
            l4.color = Color.white;

            b.color = Color.white;

            r1.color = Color.white;
            r2.color = Color.blue;
            r3.color = Color.white;
            r4.color = Color.white;
        }
        if (collision.CompareTag("R3"))
        {
            l1.color = Color.white;
            l2.color = Color.white;
            l3.color = Color.white;
            l4.color = Color.white;

            b.color = Color.white;

            r1.color = Color.white;
            r2.color = Color.white;
            r3.color = Color.blue;
            r4.color = Color.white;
        }
        if (collision.CompareTag("R4"))
        {
            l1.color = Color.white;
            l2.color = Color.white;
            l3.color = Color.white;
            l4.color = Color.white;

            b.color = Color.white;

            r1.color = Color.white;
            r2.color = Color.white;
            r3.color = Color.white;
            r4.color = Color.blue;
        }

        if (collision.CompareTag("base"))
        {
            l1.color = Color.white;
            l2.color = Color.white;
            l3.color = Color.white;
            l4.color = Color.white;

            b.color = Color.magenta;

            r1.color = Color.white;
            r2.color = Color.white;
            r3.color = Color.white;
            r4.color = Color.white;
        }
    }

}
