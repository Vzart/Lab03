// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(() => {
    LoadProdData();
    let connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();

    connection.start().then(function () {
        console.log("SignalR connection established.");
    }).catch(function (err) {
        return console.error(err.toString());
    });
    
    connection.on("LoadProducts", function () {
        LoadProdData();
    })
    
    LoadProdData();
    
    function LoadProdData() {
        let tr = '';
        $.ajax({
            url: '/Home/GetPosts',
            method: 'GET',
            success: (result) => {
                $.each(result, (k,v) => {
                    tr += `<tr>
                        <td>${v.title}</td>    
                        <td>${v.content}</td>    
                        <td>${v.publishStatus}</td>    
                        <td>${v.appUser.fullName}</td>    
                        <td>${v.postCategory.categoryName}</td>    
                        <td>
                            <a href="../Home/Edit/${v.postId}" class="btn btn-primary">Edit</a>
                            <a href="../Home/Details/${v.postId}" class="btn btn-primary">Detail</a>
                            <a href="../Home/Delete/${v.postId}" class="btn btn-danger">Delete</a>
                        </td>
                    </tr>`
                })
                
                $("#tableBody").html(tr);
            },
            error: (error) => {
                console.log(error)
            }
        });
    }
})