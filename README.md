# Test_Unimob
++++++++++++Cấu trúc Layer++++++++++++

 + Layer 1: Controller 
     GameController: chịu trách nhiệm khởi tạo game và gọi StartGame.    

 + Layer 2: System 
    - SystemStore: quản lý việc thêm, xóa và cập nhật các
      entity trong game (Tree, Client, Employer, …).
    - SystemUpgrade: định nghĩa và quản lý các loại nâng cấp trong game (IncreaseClient,IncreaseProfit, …). 
    - SystemInput: xử lý các tương tác từ player (mở Box, LevelUp, …).
    - SystemActionStore: xử lý các hành động trong gameplay (nhặt đồ, mang đồ, giao đồ, …)

  +Layer 3: Model 
    - Person: lớp cơ sở cho các đối tượng dạng nhân vật. 
    - Client: kế thừa từ Person, đại diện cho khách hàng.
    - Employer: kế thừa từ Person, đại diện cho nhân viên.
    - Box: hộp xuất hiện ban đầu trong game.
    - Tree: cây tài nguyên trong game.
    - UIView : các ui view trong game

++++++++++++Các Module / Chức năng hỗ trợ++++++++++++

-   BigNumber: định nghĩa kiểu số lớn dùng cho hệ thống kinh tế trong
    game.
-   EventBus: trung gian giao tiếp giữa các script và các layer trong hệ
    thống.
-   Menu<T>: quản lý các UI panel, bao gồm ScreenUI, PopupUI, ViewUI.
-   DataConfigUpgrade: lưu trữ dữ liệu upgrade dưới dạng local data.

++++++++++++Mục tiêu của kiến trúc++++++++++++

-   Tổ chức các class tách biệt theo chức năng để dễ quản lý và mở rộng.
-   Các layer không tham chiếu trực tiếp lên layer phía trên, việc giao
    tiếp được thực hiện thông qua EventBus.
-   Resource của game được load động từ Resources thay vì đặt sẵn trong
    Scene nhằm tối ưu quản lý và bộ nhớ.

++++++++++++Điều chỉnh Gameplay++++++++++++

-   Employer sẽ không biến mất sau khi giao hàng, mà tiếp tục thực hiện
    công việc nếu còn nhiệm vụ.
-   Có thể tăng số lượng Employer thông qua hệ thống nâng cấp.
-   Các upgrade chỉ xuất hiện khi cây tương ứng đã được mở khóa.
-   BigNumber được sử dụng để xử lý các giá trị tiền có thể tăng lên rất
    lớn.
-   Khi Client đến vị trí thì Employer mới bắt đầu phục vụ.
-   Tree được bổ sung thêm thuộc tính tốc độ sinh tài nguyên.

++++++++++++Có thể cải thiện thêm++++++++++++

-   Bổ sung Object Pooling (Pooler) để tối ưu việc tạo và hủy object
    trong game.
-   Nhân viên có thể xếp hàng chờ để đợi làm việc
-   Lưu game
