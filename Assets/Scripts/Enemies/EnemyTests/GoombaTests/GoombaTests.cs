using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GoombaTests
    {
        GameObject enemy;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            enemy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Goomba"));
            yield return null; // Wait for prefab to load
        }

        [OneTimeTearDown]
        public void TearDownOnce()
        {
            Object.Destroy(enemy);
        }

        [Test]
        public void Goom_CanLoadGoombaPrefab()
        {
            Assert.IsNotNull(enemy, "Could not load Goomba prefab");
        }

        [Test]
        public void Goomba_HasDamageableInterface()
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            Assert.IsNotNull(damageable, "Could not get IDamageable component");
        }
        [Test]
        public void Goomba_HasGoombaComponent()
        {
            Goomba goomba = enemy.GetComponent<Goomba>();
            Assert.IsNotNull(goomba, "Could not get Goomba component");
        }
        [Test]
        public void Goomba_DeadWhenTakeDamage()
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            Assert.IsNotNull(damageable, "Could not get IDamageable component");

            Goomba goomba = enemy.GetComponent<Goomba>();
            Assert.IsNotNull(goomba, "Could not get Goomba component");

            damageable.TakeDamage(1f); // Pick any number
            Assert.IsTrue(goomba.IsDead, "Goomba is not dead");
        }
    }
}
