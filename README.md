綫上麥當勞點餐使用説明
==
操作影片：<br>
==
[![](https://github.com/Yang027/ASP.Net-MVC-Macdonald-Order-System/blob/main/qrcode_www.youtube.com.png)](https://www.youtube.com/watch?v=LaAgc6GFQk0)  
"使用説明"<br>
賬號説明
==
沒有賬號：<br>
-可以注冊賬號，注冊的賬號預設等級為一般顧客<br>
已有的賬號説明：
賬號分爲兩種不同的等級，不同等級擁有不同的權限，本點餐系統一共有兩個等級：
`一般顧客` 和 `系統管理員` <br>
1.一般顧客的權限：<br>
-購買餐點<br>
-更改名字<br>
-查詢訂單<br>
-查詢折價券<br>

2.系統管理員的權限：<br>
`Account:Admin`<br>
`password:admin`<br>
-購買餐點<br>
-新增商品<br>
-查詢所有人的訂單<br>
-設定折價券<br>

點餐説明
==
-可以依照自己喜好選擇菜單的目錄別，然後在直接選擇要買的東西，點擊加入購物車即可<br>
-重複購買的商品，也會自己加入購物車<br>

購物車説明
==
-顯示你所點的餐點<br>
-可以刪除你不想要的餐點<br>
-可使用折價券：如果是不存在的折價券則不能用<br>
-顯示你購買的金額<br>
-按結賬鍵完成購買<br>
-一定要登錄后才能結賬<br>

資料庫
==
在`App_Data`下，一共有三張表：<br>
`tAccount`儲存賬號<br>
`tItem`儲存商品，可以使用admin權限新增/刪除/修正，商品<br>
`tdiscount`使用admin權限增加折扣碼，供使用者使用<br>

更新：歷史訂單加上了pagelist，更加整潔
