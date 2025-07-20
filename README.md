# SummerGameJam
- 7.20 Hankun
- 我这边新添加了6个脚本，都是关于食材区域的。
- -首先最重要的脚本是EnemyManager，这个脚本跟EnemyMovement相关联，里面包含了除了冰块以外所有的怪物的功能，移动，血量，速度，掉落物。你直接把EnemyManager绑定给一个空的game object就可以实现功能。然后你就可以添加想要的信息在里。在Test Scene里面我挂载到同名（EnemyManager）的文件下面的了。Spawn Area就是带有collider2D的食材移动区域
- 然后就是IceBlock这个代码，因为冰块的功能有些不一样我就单独拿出来写了，在Test Scene中脚本是挂载到Ice Monster上的，具体功能就是玩家靠近冰块右键就可以获取冰块了
- 然后是Bullet和BulletShooter这两个脚本，都是player射击的功能的。BulletShooter直接绑定在Test-player上面的。子弹需要带有tag“bullet”
- 最后是InventoryManager，这个代码你厨房区域应该会用到，这个是玩家仓库的代码，就是玩家收集的食材冰块的数据都会保存显示在里面
- 然后你run游戏走到食材区域就可以点按左键射击食材，右键到冰块附近去打冰块了，然后收集的物品信息会显示在右下角。
