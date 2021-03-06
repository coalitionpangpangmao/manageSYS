﻿
$(document).ready(function () {
    $("#browser").treeview({
        toggle: function () {
            console.log("%s was toggled.", $(this).find(">span").text());
        },
        persist: "cookie",
        collapsed: true
    });
    showtempchart();
    // activate the button
    $('#btnCompare').click(function () {
        $('#statics').empty();
        showPointPlot();
    });
});

function showPointPlot() {   
    var chart = $('#container').highcharts();
    var globalAxis = [];
    var globalTitle = [];
    var globalYAxis = [];
    var gt =[];
    var count = 0;
    while (chart.series.length) {
        chart.series[0].remove(false);
    }    
    var paralist = [];
    var paranum = 0;
    $("#cklistPara tbody").find("tr").each(function(){
       
        var point = $(this).find('input').val();
        if(paralist.indexOf(point)== -1)
        {
            paralist.push(point);
            paranum++;
        }
    });
    //  if ($('#browser').find('input[checked=true]').length == 1) {
    if (paranum == 1) {
        $("#cklistPara tbody").find("tr").each(function () {
            var item = $(this).find('label').text();;
            var json = {
                "type": "Para",
                "point": $(this).find('input').val(),
                "startTime": item.substr(item.indexOf('_') + 1, 19),
                "stopTime": item.substr(item.indexOf('~') + 1)
            }
            
            $.ajax({
                type: "POST",
                url: "../Response/RealDataHandler.ashx",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                dataType: "json",
                success: function (result) {
                    var chart =  $('#container').highcharts();
                    if (chart.series.length == 0) {
                        DrawChartWithStd(result[0]);
                        if (result[0]["statics"]) {
                            $('#statics').empty();
                            $('#statics').append(result[0]["statics"].toString());
                        }                        
                    }
                    else {
                        chart = $('#container').highcharts();
                        var xAxis = result[0]["xAxis"];
                        var yAxis = result[0]["yAxis"];
                        var para_name = result[0]["pointname"].toString();
                        chart.addSeries({
                            name: para_name,
                            data: yAxis
                        });
                        if (result[0]["statics"])
                            $('#statics').append(result[0]["statics"].toString());
                    }
                },
                error: function (message) {
                    $("#request-process-patent").html("从服务器获取数据失败！");
                }
            });

        });
    }
    else {
        drawPicture();
        var chart = $('#container').highcharts();
        $("#cklistPara tbody").find("tr").each(function (ele, index) {
            var item = $(this).find('label').text();
            var json = {
                "type": "Para",
                "point": $(this).find('input').val(),
                "startTime": item.substr(item.indexOf('_') + 1, 19),
                "stopTime": item.substr(item.indexOf('~') + 1)
            }
            /*$.ajax({
                type: "POST",
                url: "../Response/RealDataHandler.ashx",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                dataType: "json",
                success: function (result) {
                    debugger;
                    var xAxis = result[0]["xAxis"];
                    var yAxis = result[0]["yAxis"];
                    var para_name = result[0]["pointname"].toString();
                    chart.addSeries({
                        name: para_name,
                        data: yAxis
                    });
                    if (result[0]["statics"])
                        $('#statics').append(result[0]["statics"].toString());
                },
                error: function (message) {
                    $("#request-process-patent").html("从服务器获取数据失败！");
                }
            });*/

            $.ajax({
                type: "POST",
                url: "../Response/RealDataHandler.ashx",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                dataType: "json",
                success: function (result) {
                    if (result == null)
                        return;
                    count++;
                    debugger;
                    globalTitle.push(result[0]["pointname"] || "");
                    var xAxis = result[0]["xAxis"] || [];
                    globalAxis = globalAxis.concat(xAxis);
                    gt.push(result[0]["xAxis"]);
                    //globalAxis = globalAxis.concat(xAxis);
                    //globalAxis = globalAxis.sort();
                    //globalAxis = Array.from(new Set(globalAxis.sort()));
                    globalAxis = Array.from(new Set(globalAxis.sort()));
                    globalAxis = (globalAxis.concat(globalAxis)).sort();
                    globalYAxis.push(result[0]["yAxis"] || []);
                    if (count === paranum) {

                            drawPicture(globalAxis);
                            chart = $('#container').highcharts();
                            globalYAxis.forEach((item, i) => {
                                item = alignData(item, gt[i],globalAxis);
                                chart.addSeries({
                                    name: globalTitle[i],
                                    data: item
                                })
                            });
                        }
                    if(result[0]["statics"])
                        $('#statics').append(result[0]["statics"].toString());
                },

                error: function (message) {
                    $("#request-process-patent").html("从服务器获取数据失败！");
                }
            });

        });
    }     
   
};

function alignData(data, time, ctime){
    let result = Array(ctime.length).fill(null);
    for(let i=0; i<time.length;i++){
        let p1 = ctime.indexOf(time[i]);
        let p2 = ctime.lastIndexOf(time[i]);
        if((i+1)<time.length && time[i]==time[i+1]){
            result[p1] = data[i];
            result[p2] = data[i+1];
            i++;
    }else{
        result[p2] = data[i]
}

}
return result;
}

function refreshChart() {
    var chart = $('#container').highcharts();
    while (chart.series.length) {
        chart.series[0].remove(false);
    }
    chart.addSeries({
        name: 'temprature1',
        marker: {
            symbol: 'square'
        },
        data: [7.0, 6.9, 9.5, 10, 18.2, 21.5, 25.2, {
            y: 26.5,
            marker: {
                symbol: 'url(https://www.highcharts.com/samples/graphics/sun.png)'
            }
        }, 23.3, 18.3, 13.9, 9.6]

    });
    chart.addSeries({
        name: 'temprature2',
        marker: {
            symbol: 'diamond'
        },
        data: [{
            y: 3.9,
            marker: {
                symbol: 'url(https://www.highcharts.com/samples/graphics/snow.png)'
            }
        }, 4.2, 5.7, 8.5, 11.9, 9, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8]
    });
}



function treeClick(obj) {
    var code = obj.value;
    if (obj.checked) {
        $("#hidecode").val(code);
        $("#btnAdd").click();
    }
    else {
        $("#hidecode").val(code);
        $("#btnDel").click();
    }
}
function showtempchart() {
    drawPicture();
    var chart = $('#container').highcharts();
    
    chart.addSeries({
        name: 'temprature1',
        marker: {
            symbol: 'square'
        },
        data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, {
            y: 26.5,
            marker: {
                symbol: 'url(https://www.highcharts.com/samples/graphics/sun.png)'
            }
        }, 23.3, 18.3, 13.9, 9.6]

    });
    chart.addSeries({
        name: 'temprature2',
        marker: {
            symbol: 'diamond'
        },
        data: [{
            y: 3.9,
            marker: {
                symbol: 'url(https://www.highcharts.com/samples/graphics/snow.png)'
            }
        }, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8]
    });
}
function drawPicture(xAxis) {
    // create the chart
    $('#container').highcharts({
        chart: {
            events: {
                addSeries: function () {
                    var label = this.renderer.label('A series was added, about to redraw chart', 100, 120)
                                .attr({
                                    fill: Highcharts.getOptions().colors[0],
                                    padding: 10,
                                    r: 5,
                                    zIndex: 8
                                })
                                .css({
                                    color: '#FFFFFF'
                                })
                                .add();

                    setTimeout(function () {
                        label.fadeOut();
                    }, 1000);
                }
            }
        },
        xAxis:{
            labels:{
                overflow:'justify'
            },
            type:'datetime',
            categories:xAxis,
            crosshair:true
        },
        tooltip: {
            pointFormat: '<span style="color:{series.color}">{series.name}</span>: {point.y}<br/>',
            shared: true
        },

        title: { text: '过程数据比对' },
        yAxis: { title: { text: '值' } },
        legend: { layout: 'vertical', align: 'right', verticalAlign: 'middle' },
        plotOptions: { series: { cursor: 'pointer', pointStart: 0 } },
        credits: {
            enabled: false
        },
        responsive: {
            rules: [{
                condition: { maxWidth: 500 },
                chartOptions: {
                    legend: { layout: 'horizontal', align: 'center', verticalAlign: 'bottom' }
                }
            }]
        }
    });

}

function DrawChartWithStd(result) {  
    var upper = result["upper"];
    var lower = result["lower"];
    var value = result["value"];
    var errdev = result["errdev"];
    var xAxis = result["xAxis"];
    var yAxis = result["yAxis"];
    var max = result["max"];
    var min = result["min"];
    if (max < upper + (upper - value))
        max = upper + (upper - value);
    if (min > lower - (value - lower))
        min = lower - (value - lower);
    var para_name = result["para_name"];
    if (xAxis == null) {
        alert('没有从数据库获取数据，请查看质量采集设置是否正确，或IH实时数据库是否正常！！');
        return;
    }
    var chart = new Highcharts.Chart({
        chart: {
            renderTo: 'container',
            type: 'spline'
        },
        title: {
            text: '数据点趋势图'
        },
        xAxis: {
            type: 'datetime',
            labels: {
                overflow: 'justify'
            },
            categories: xAxis == null ? [1, 2, 3, 4, 5, 6] : xAxis,
            crosshair: true
        },
        yAxis: {
            minorGridLineWidth: 0,
            gridLineWidth: 0,
            alternateGridColor: null,
            //min:min,
            //max:max,
            tickPositioner: function () {
                let position = [];
                let interval = (upper - lower) / 10;

                let start = lower - 10 * interval;
                position.push(start);
                position.push((upper - lower) / 2 + lower);
                position.push(upper + 10 * interval);
                return position;
            },
            plotBands: [{ // Light air
                from: Number.NEGATIVE_INFINITY,
                to: lower,
                color: 'rgba(255,0, 0, 0.4)',
                label: {
                    text: 'Fault',
                    style: {
                        color: '#606060'
                    }
                }
            }, { // Light breeze
                from: lower,
                to: lower + errdev / 100 * (value - lower),
                color: 'rgba(255, 255, 0, 0.4)',
                label: {
                    text: 'Warning',
                    style: {
                        color: '#606060'
                    }
                }
            }, { // Gentle breeze
                from: lower + errdev / 100 * (value - lower),
                to: upper - errdev / 100 * (upper - lower),
                color: 'rgba(0,255, 0, 0.4)',
                label: {
                    text: 'good',
                    style: {
                        color: '#606060'
                    }
                }
            }, { // Moderate breeze
                from: upper - errdev / 100 * (upper - lower),
                to: upper,
                color: 'rgba(255, 255, 0, 0.4)',
                label: {
                    text: 'Warning',
                    style: {
                        color: '#606060'
                    }
                }
            }, { // Fresh breeze
                from: upper,
                to: Number.POSITIVE_INFINITY,
                color: 'rgba(255,0, 0, 0.4)',
                label: {
                    text: 'fault',
                    style: {
                        color: '#606060'
                    }
                }
            }]
        },
        credits: {
            enabled: false // remove high chart logo hyper-link  
        },
        plotOptions: {
            spline: {
                lineWidth: 4,
                states: {
                    hover: {
                        lineWidth: 5
                    }
                },
                marker: {
                    enabled: false
                }
                //                                pointInterval: 30000, // one hour
                //                                pointStart: Date.UTC(2018, 4, 31, 0, 0, 0)
            }
        },
        series: [{ name: para_name, data: yAxis, tooltip: { value: ' ' } }]
    });
}
