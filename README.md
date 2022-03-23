# コンソールオセロ
CPU同士を対戦させる4人対戦オセロです。
IPlayerインターフェースを実装して対戦させます。

実装するのはIPlayer.Placeメソッドで、盤面の状態を受け取って、自分が石を置く場所を返すだけです。

どんな戦略にするかは自由です。

実装したらそのクラスをProgram.cs内の以下の場所でgamManager.Playersに追加してください。4人以上にはしないでください。

```
gameManager.Players.Add(new Random_CPU("スズキ"));
gameManager.Players.Add(new Random_CPU("タカハシ"));
gameManager.Players.Add(new Random_CPU("ヤマダ"));
gameManager.Players.Add(new Random_CPU("サトウ"));
```

# ルール

1. 各プレイヤーはライフを3点持っています。これが0になったらリタイヤです。
2. すでに置かれていたり、盤外に置こうとしたり、ひっくり返す石がないマスに置こうとすると、お手つきとしてライフが-1されます。
3. 今の所、終了条件があなだらけですが、全てのマスに石が置かれたときに一番スコアが高い人が勝ちです。
4. プレイヤーは4人です。多くても少なくてもいけません。4人です。

# 実行画面
https://user-images.githubusercontent.com/43431002/159196918-426567a2-35dd-4bd5-a3f9-9d95c24aeba9.mp4

# IPlayer.Placeメソッドの入力と出力

## 入力
`Place`メソッドが受け取るパラメータは`List<List<PlayerID>>`型の`board`です。
  
これはデフォルトでは盤面を表す`10x10`のリストです。
  各セルの状態はPlayerID列挙型の値で表され、以下の5つの値のいずれかで埋められています。
  
  ```
  Player1 : Player1の石が置かれている
  Player2 : Player2の石が置かれている
  Player3 : Player3の石が置かれている
  Player4 : Player4の石が置かれている
  None : 何も置かれていない。
  ```
  
  自分が何Playerなのかは、gamaManagerによってIPlayer.IDプロパティにセットされているので、これを参照して自分と他のプレイヤーを区別してください。
  
  
  ## 出力
 `Place`メソッドは石を置く位置を表す`(int row, int column)`のタプルを返すようにします。
  盤面のデータを元に自分が次に石を置く位置が決まったら、それを例えば`(1,3)`のように返します。
  
  
  
 
  
