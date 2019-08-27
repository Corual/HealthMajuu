﻿




$(function () {

    function pageControl(total, count, current, flag) {
        var flagObj = new Object();
        flagObj.flag = flag;
        page_ctrl({
            obj_box: '.page_1', //翻页容器
            total_item: total, //条目总数
            per_num: count, //每页条目数
            current_page: current, //当前页
            change_content: function change_content(per_num, current_page) {
                //per_num = per_num ? per_num : 10;
                //current_page = current_page ? current_page : 1;



                if (per_num == current_page && flagObj.flag) { return; }

                //console.log(per_num);
                //console.log(current_page);

                axios.post(
                    '/Machine/Paging/' + current_page,
                )
                    .then(function (response) {
                        //loader.stop();
                        //THIS.removeAttr('disabled');
                        //console.log(response);
                        var serverData = response.data;
                        pageDataLoad(serverData);

                        serverData.businessResult ?
                            (flagObj.flag = false) :  //notify(serverData.msg, 'success', function () { location.reload(); }) :
                            serverData.total > 0 ? notify(serverData.msg, 'error') : '';
                    })
                    .catch(function (error) {
                        //console.log(error);
                        //loader.stop();
                        //THIS.removeAttr('disabled');
                        notify(error, 'warning');
                    });
            }
        });
    }

    function notify(msg, notifyTpye, callAction) {
        msg = msg || '';
        notifyTpye = notifyTpye || 'success';
        callAction = callAction || function () { };
        new NoticeJs({
            text: msg,
            position: 'middleCenter',
            type: notifyTpye, //error， warning， info， success
            modal: true,
            animation: {
                open: 'animated lightSpeedIn',
                close: 'animated lightSpeedOut'
            },
            callbacks: {
                onClose: [callAction]
            }
        }).show();
    }

    function pageDataLoad(pageData) {
        //console.log(pageData);
        if (!pageData.businessResult) {
            return false;
        }
        $('#dataBody').children().remove();
        var html = new Array();
        for (var i = 0; i < pageData.pageData.length; i++) {
            var itme = pageData.pageData[i];
            html.push('<tr>');
            html.push('   <td>' + (itme.id) + '</td>');
            html.push('   <td>' + (itme.ipAddressV4) + '</td>');
            html.push('   <td>' + (itme.remarks || '') + '</td>');
            html.push('</tr>');
        }

        $('#dataBody').html(html.join(''));
    }

    function getLoding(invoker) {
        return Ladda.create(invoker);
    }

    if (loadPageInfo.businessResul>0) {
        pageControl(loadPageInfo.total, loadPageInfo.eachPageDataCount, loadPageInfo.current, true);
    }

    var importDataButton = $('#import');
    var exportDataButton = $('#export');
    var exlFile = $('#exlFile');
    var importLoader = null;

    exlFile.change(function (e) {
        //console.log(e);
        console.log(this.files);

        if (this.files.length <1)
        {
            
            importLoader.stop();
            exportDataButton.removeAttr('disabled');
            importDataButton.removeAttr('disabled');
            return;
        }

        var fileName = this.files[0].name;
        var index = fileName.lastIndexOf(".");
        var suffix = fileName.substring(index + 1);
        var suffixCollection = ['xlsx'];//['xls', 'xlsx', 'csv'];
        if (suffixCollection.indexOf(suffix) == -1)
        {
            //notify('你上传的不是Excel文件', 'error');
            notify('你上传的不是*.xlsx后缀的l文件', 'error');
            importLoader.stop();
            exportDataButton.removeAttr('disabled');
            importDataButton.removeAttr('disabled');
            return;
        }

        var fd = new FormData();
        fd.append('file', this.files[0]);
        //console.log(fd.get('file'))

        axios({
            method: 'post',
            url: '/Machine/Import',
            headers: { 'Content-Type': 'multipart/form-data' },
            data:fd
        })
            .then(function (response) {
                importLoader.stop();
                exportDataButton.removeAttr('disabled');
                importDataButton.removeAttr('disabled');
                var serverData = response.data;

                serverData.businessResult ?
                    notify(serverData.msg, 'success', function () { location.reload(); }) :
                    notify(serverData.msg, 'error');

            })
            .catch(function (error) {
                importLoader.stop();
                exportDataButton.removeAttr('disabled');
                importDataButton.removeAttr('disabled');
                notify(error, 'warning');
            });

        return;
    });

    importDataButton.click(function () {
        exportDataButton.attr('disabled', 'disabled');
        var THIS = $(this);
        importLoader = getLoding(this);
        importLoader.start();
        exlFile.click();

    });

    exportDataButton.click(function () {
        importDataButton.attr('disabled', 'disabled');
        var THIS = $(this);
        var loader = getLoding(this);
        loader.start();

        axios.post('/Machine/Export', {} )
            .then(function (response) {
                loader.stop();
                THIS.removeAttr('disabled');
                var serverData = response.data;
                serverData.businessResult ?
                    notify(serverData.msg, 'success', function () { location.reload(); }) :
                    notify(serverData.msg, 'error');
            })
            .catch(function (error) {
                loader.stop();
                THIS.removeAttr('disabled');
                notify(error, 'warning');
            });

    });
});