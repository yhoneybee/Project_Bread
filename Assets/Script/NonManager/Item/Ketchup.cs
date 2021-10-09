using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ketchup : Item
{
    bool first = true;
    float add = 0;

    public override void Equip()
    {

    }

    public override void Ingame()
    {
        first = true;
        add = 0;
    }

    public override void OnAttack(Unit taken)
    {
    }

    public override void OnHit(Unit take, ref float damage)
    {
        if (first)
        {
            first = false;
            print("Start");
            //StartCoroutine(EADUpFor3sec());
            Owner.GetComponent<MonoBehaviour>().StartCoroutine(EADUpFor3sec());
        }
    }

    public override void UnEquip()
    {
    }

    IEnumerator EADUpFor3sec()
    {
        print("IN");
        var wait = new WaitForSeconds(10);

        int[] dp = new int[30];

        dp[0] = 0;
        dp[1] = 1;
        dp[2] = 1;
        dp[3] = 2;

        int idx = 2;

        while (Owner.Stat.HP > 0)
        {
            ++idx;
            dp[idx] = dp[idx - 1] + dp[idx - 2];
            print($"dp[{idx}] = {dp[idx]}");
            Owner.Stat.AD += dp[idx];
            add += dp[idx];
            yield return wait;
        }

        Owner.Stat.HP -= add;

        // 0 1 1 2 3 5 8 13
    }
}
