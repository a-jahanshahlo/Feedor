function CreateNode() {
    //  var ref = $("tree").jstree('create_node', '#', { 'attr': { 'id': newId }, 'text': new Name }, 'last');
    $.post('/Tree/GetAjaxList',
    {
        'id': 1//operation
    })
    .done(function (result) {

    })
    .fail(function (result) {
        alert('failed.....');

    });
}

function LogoSiteBind(id) {

    var node = $.jstree.reference(id);//._get_parent(1);
    var parent = node._model.data[id];
    var logoSite = $("#logoSite");
    $(logoSite).empty();
    var txt = "<div>" + parent.text + " </div>";
    var imgbtn = "";
    var refreshbtn = "<button id='refreshbtn' value=''  disabled='disabled' type='button'  class='btn btn-link'><i class='fa  fa-refresh fa-2x  fa-inverse'></i>  </button >";
    var visitedbtn = "<button id='visitedbtn' value=''  disabled='disabled' type='button'  class='btn btn-link'><i class='fa  fa-check-circle fa-2x  fa-inverse'></i>  </button >";
    var obj0 = "<div class='col-sm-2 img-responsive' style='background: none repeat scroll 0% 0% #FF7E2D;padding: 5px;line-height: 50px;'> " + visitedbtn + refreshbtn + "</div>";
    var obj2 = "<div class='col-sm-9 ' style='color: white;font-size: 20px;line-height: 60px;background: none repeat scroll 0% 0% #499ba9; border-right-color: #6fcddd;border-right-style:solid;border-width: 1px;'>" + txt + " </div>";
    var obj1 = "<div class='col-sm-1' style='background: none repeat scroll 0% 0% #5fbdcd;padding: 5px;'> <img src='/content/images/www50.png' width='50' height='50' class='img-responsive center-block' /></div>";


    $(logoSite).append(obj1);
    $(logoSite).append(obj2);
    $(logoSite).append(obj0);

}

function CreateItems(result) {

    var items = result.Items;

    var maincontainr = $("#feedItem");
    $(maincontainr).empty();
    $(items).each(function (index, el) {
        var h2 = "<h4 class='plantop'>" + el.Title + "</h4>";
        var parag = "<p>" + el.Description + "</p>";
        var button = " <p class='lead'><a href='" + el.Links[0].Url + "' class='btn btn-default'>ادامه مطلب</a></p>";
        var li = "";
        $(el.Authors).each(function (i, author) {
            li += "<li><b>ایمیل:</b>" + author.Email + "</li>";
            li += "<li><b>نام:</b>" + author.Name + "</li>";
        });
        li += "<li><b>تاریخ ارسال:</b>" + el.PubDate + "</li>";

        var liLink = "";
        $(el.Links).each(function (ii, link) {
            liLink += "<li><a href='" + link.Url + "'>لینک کمکی</a></li>";

        });
        var libtn = "<li><button value='" + el.UniqId + "' type='button' class='insert btn btn-success btn-xs'>ذخیره</button><li>";
        libtn += "<li><button value='" + el.UniqId + "' type='button' class='delete btn btn-danger btn-xs'>حذف</button><li>";
        var ul = "<ul class='list-inline'>" + li + liLink + libtn + "</ul>";
        //   $(ul).append("<li>Pub:" + el.PubDate + "</li>");
        $(maincontainr).append("<div><div class='row box2'><div class='col-xs-12'>" + h2 + parag + button + ul + "</div></div><hr/></div>");
        // $(el).addClass("newClass");
    });

}
jQuery(document).ready(function ($) {
    /**********************Upload file****************/
    $('#fileupload').fileupload({
        dataType: 'json',
        url: '/File/UploadFiles',
        autoUpload: true,
        add: function (e, data) {
            data.context = $('<button class="btn btn-success"/>').text('ارسال')
                .appendTo("#filename")
                .click(function () {
                    data.context = $('<p/>').text('در حال ارسال...').replaceAll($(this));
                    data.submit();
                });
        },
        done: function (e, data) {
           // $('.file_name').html(data.result.name);
            //$('.file_type').html(data.result.type);
            //$('.file_size').html(data.result.size);
         
                data.context.text('ارسال با موفقیت انجام شد');
         
        },
        fail: function (e, data) {
            data.context.text('خطا در ارسال فایل');
            //$('.file_type').html(data.result.type);
            //$('.file_size').html(data.result.size);
        }
    }).on('fileuploadprogressall', function (e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('.progress .progress-bar').css('width', progress + '%');
    });

    /*********************End Uplad file**********************/
    $('.modal').on('shown.bs.modal', function () {
      
    });
    $('.modal').on('hidden.bs.modal', function () {
        $(this).find('.file_name').html('');
    });

    $('body').on('click', '#visitedbtn', function () {
        var channelid = $(this).attr("value");
        $.post("/Channel/UpdateChannel", { id: channelid })
                .done(function (data) {
                    $('#jstree').jstree('refresh');
                }).fail(function (result) {
                   
                });
    });
    $('body').on('click', '#refreshbtn', function () {
        var selectedid = $(this).attr("value");
        $('#jstree').jstree("select_node", selectedid);
        $('#jstree').jstree('refresh');
    });
    $('body').on('click', '.delete', function () {
        var elem = this;
        $(this).attr('disabled', 'disabled');
        $.post("/Item/RemoveItem", { id: $(this).attr("value") })
                .done(function (data) {
                    $(elem).parent().parent().parent().parent().parent().fadeOut("slow");
                }).fail(function (result) {
                    $(elem).removeAttr('disabled');
                });
    });
    $('body').on('click', '.insert', function () {
        var elem = this;
        $(this).attr('disabled', 'disabled');
        $.post("/Item/AddItem", { id: $(this).attr("value") })
                .done(function ( data) {
                    $(elem).parent().parent().parent().parent().parent().fadeOut("slow");
                }).fail(function (result) {
                    $(elem).removeAttr('disabled');
                });
    });
    $("#loadbtn").click(function () {
        $(this).attr('disabled', 'disabled');
        $.post("/Tree/GetMoreList", {})
        .done(function (data) {
            $(data).each(function (index, item) {
                var ref = $("#jstree").jstree('create_node', '#', { 'attr': { 'id': item.id }, 'text': item.text }, 'last');


            });
            $('#jstree').jstree('refresh');
            $("#loadbtn").removeAttr('disabled');
        }).fail(function (result) {
            $("#loadbtn").removeAttr('disabled');
        });
    });
    var selectedData;
    $('#jstree').jstree({
        "core": {
            "multiple": false,
            "check_callback": false,
            'data': {
                'url': '/Tree/GetTreeJson',
                "type": "POST",
                "dataType": "json",
                "contentType": "application/json; charset=utf8",
                'data': function (node) {
                    return { 'id': node.id };
                }
            },
            'themes': {
                'variant': 'small',
                'stripes': true
            }
        },
        "types": {
            "default": {
                "icon": '@Url.Content("~/Content/images/bookmark_book_open.png")'
            },
        },
        "plugins": [/*"contextmenu", */ "dnd", "state", "types", "wholerow", "sort", "unique"],
        "contextmenu": {
            "items": function (o, cb) {
                var items = $.jstree.defaults.contextmenu.items();
                items["create"].label = "ایجاد زیر شاخه";
                items["rename"].label = "تغییر نام";
                items["remove"].label = "حذف";
                var cpp = items["ccp"];
                cpp.label = "ویرایش";
                var subMenu = cpp["submenu"];
                subMenu["copy"].label = "کپی";
                subMenu["paste"].label = "پیست";
                subMenu["cut"].label = "برش";
                return items;
            }
        }
    }).on('select_node.jstree', function (e, data) {
        var nodeid = data.node.id;
        selectedData = data.selected[0];
        LogoSiteBind(data.node.parent);
        // alert(dara);
        $.post('/Tree/GetAjaxList', { id: selectedData })
      .done(function (result) {
          var onDone = function (result1) {
              // throw new Error("Not implemented");
              CreateItems(result1);

              $("#refreshbtn").attr("value", nodeid);
              $("#refreshbtn").removeAttr('disabled');
              $("#visitedbtn").removeAttr('disabled');
              $("#visitedbtn").attr("value", result1.Id);
          };
          onDone(result);
      })
      .fail(function (result) {
          var onFail = function (result1) {
              // throw new Error("Not implemented");
          };
          onFail(result);
      });
    }).bind('loaded.jstree', function (e, data) {
        $("#loadbtn").removeAttr('disabled');
    });


});