




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
        try {
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
        } catch (e) {
            console.log(e);
        }
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
        //console.log(this.files);

        exportDataButton.attr('disabled', 'disabled');
        importDataButton.attr('disabled', 'disabled');
        importLoader = getLoding(importDataButton[0]);
        importLoader.start();

        var fileField = this;
        if (fileField.files.length <1)
        {
            
            importLoader.stop();
            exportDataButton.removeAttr('disabled');
            importDataButton.removeAttr('disabled');
            return;
        }

        var fileName = fileField.files[0].name;
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
        fd.append('file', fileField.files[0]);
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
                exlFile.val('');
                var serverData = response.data;

                serverData.businessResult ?
                    notify(serverData.msg, 'success', function () { location.reload(); }) :
                    notify(serverData.msg, 'error');

            })
            .catch(function (error) {
                importLoader.stop();
                exportDataButton.removeAttr('disabled');
                importDataButton.removeAttr('disabled');
                exlFile.val('');
                notify(error, 'warning');
            });

        return;
    });

    importDataButton.click(function () {
  
        exlFile.click();

    });

    exportDataButton.click(function () {
        importDataButton.attr('disabled', 'disabled');
        var THIS = $(this);
        var loader = getLoding(this);
        loader.start();

        axios.post('/Machine/Export', {}, { responseType: 'arraybuffer'})
            .then(function (response) {
                loader.stop();
                exportDataButton.removeAttr('disabled');
                importDataButton.removeAttr('disabled');
                if (response.headers['content-type'] == 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') {

                    var url = window.URL.createObjectURL(new Blob([response.request.response]))
                    var link = document.createElement('a')
                    link.style.display = 'none'
                    link.href = url
                    link.setAttribute('download', (new Date()).valueOf()+'.xlsx')
                    document.body.appendChild(link)
                    link.click();
                    return false;
                }

                var text = utf8ByteToUnicodeStr(Array.prototype.slice.call(new Uint8Array(response.data)));
                var serverData  = JSON.parse(text);
                serverData.businessResult ?
                    notify(serverData.msg, 'success') :
                    notify(serverData.msg, 'error');
            })
            .catch(function (error) {
                loader.stop();
                exportDataButton.removeAttr('disabled');
                importDataButton.removeAttr('disabled');
                notify(error, 'warning');
            });

    });

    function utf8ByteToUnicodeStr(utf8Bytes) {
        var unicodeStr = "";
        for (var pos = 0; pos < utf8Bytes.length;) {
            var flag = utf8Bytes[pos];
            var unicode = 0;
            if ((flag >>> 7) === 0) {
                unicodeStr += String.fromCharCode(utf8Bytes[pos]);
                pos += 1;

            } else if ((flag & 0xFC) === 0xFC) {
                unicode = (utf8Bytes[pos] & 0x3) << 30;
                unicode |= (utf8Bytes[pos + 1] & 0x3F) << 24;
                unicode |= (utf8Bytes[pos + 2] & 0x3F) << 18;
                unicode |= (utf8Bytes[pos + 3] & 0x3F) << 12;
                unicode |= (utf8Bytes[pos + 4] & 0x3F) << 6;
                unicode |= (utf8Bytes[pos + 5] & 0x3F);
                unicodeStr += String.fromCharCode(unicode);
                pos += 6;

            } else if ((flag & 0xF8) === 0xF8) {
                unicode = (utf8Bytes[pos] & 0x7) << 24;
                unicode |= (utf8Bytes[pos + 1] & 0x3F) << 18;
                unicode |= (utf8Bytes[pos + 2] & 0x3F) << 12;
                unicode |= (utf8Bytes[pos + 3] & 0x3F) << 6;
                unicode |= (utf8Bytes[pos + 4] & 0x3F);
                unicodeStr += String.fromCharCode(unicode);
                pos += 5;

            } else if ((flag & 0xF0) === 0xF0) {
                unicode = (utf8Bytes[pos] & 0xF) << 18;
                unicode |= (utf8Bytes[pos + 1] & 0x3F) << 12;
                unicode |= (utf8Bytes[pos + 2] & 0x3F) << 6;
                unicode |= (utf8Bytes[pos + 3] & 0x3F);
                unicodeStr += String.fromCharCode(unicode);
                pos += 4;

            } else if ((flag & 0xE0) === 0xE0) {
                unicode = (utf8Bytes[pos] & 0x1F) << 12;;
                unicode |= (utf8Bytes[pos + 1] & 0x3F) << 6;
                unicode |= (utf8Bytes[pos + 2] & 0x3F);
                unicodeStr += String.fromCharCode(unicode);
                pos += 3;

            } else if ((flag & 0xC0) === 0xC0) { //110
                unicode = (utf8Bytes[pos] & 0x3F) << 6;
                unicode |= (utf8Bytes[pos + 1] & 0x3F);
                unicodeStr += String.fromCharCode(unicode);
                pos += 2;

            } else {
                unicodeStr += String.fromCharCode(utf8Bytes[pos]);
                pos += 1;
            }
        }
        return unicodeStr;
    }
});