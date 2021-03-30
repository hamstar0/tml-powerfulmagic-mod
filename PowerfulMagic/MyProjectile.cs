using System;
using Terraria;
using Terraria.ModLoader;


namespace PowerfulMagic {
	class PowerfulMagicProjectile : GlobalProjectile {
		public override void SetDefaults( Projectile projectile ) {
			if( projectile.magic && !projectile.npcProj /*&& projectile.owner >= 0*/ ) {
				foreach( Player plr in Main.player ) {
					if( plr?.active == true && !plr.dead ) {
						var myplayer = plr.GetModPlayer<PowerfulMagicPlayer>();

						if( myplayer.ClaimNextMagicProjectile >= 1 ) {
							myplayer.ClaimNextMagicProjectile--;

							projectile.scale *= 2f;
						}
					}
				}
			}
		}
	}
}
