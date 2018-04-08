/**
* @description: 分类
* @author: szj (2017-04-20 10:35:08)
* @update: szj () 
*/

/*
 * 使用方法
 * 1、调用方法
 *  $(document).on("click",".txtsend",function(){
 *  	 //this 触发元素的对象
 *       handleChange(this,dataview);
 *	});
 * 2、页面会生成<input type="hidden" id="hid_txtsend" code="" value="">
 *   其中  code为  dataview中的code，value为 dataview中的value。
 *  (hidde的id规则：hid_***,***为触发事件的元素id,如 hid_txtsend)
 * 3、数据源格式 code、label必填，value可为空;如存在下一级 children节点必有，不存在下一级，则没有children节点。
 * **/

//数据源格式
var data1="[{code:'368',value: '368',label: '壁纸',children: [{code:'1408',value: '1408',label: '墙纸',children: [{code:'1411',value: '1411',label: '贴纸'}]},{code:'1409',value: '1409',label: '天花板',children: [{code:'1410',value: '1410',label: '小型'}]}]}]";
//当前对像,当前显示的div,当前对像id,选中的元素
var $e,$currdiv,$currid,$data,$span;
(function($){
	/**
	 * 分类调用方法
	 * @method handleChange
	 * @param {object} evg this对象,{json数组} data 分类参数
	 * @return 
	 * @author: szj (2017-04-20 10:35:08)
	 */
	handleChange=function(arg,data){
	  $e=$(arg);
	  $currid = typeof ($e.attr("id")) == "undefined" ? "" : $e.attr("id");
      $span=$("#"+$currid).next("span");
	  dataView=eval(data);
	  init();
   },
   /**
    * 初始化
    * @method init
    * @param 
    * @return 
    * @author: szj (2017-04-20 10:35:08)
    */
	init=function(){
	   $currdiv=$('div[xplacement="'+$currid+'"]');
	   var $hid=$("#hid_"+$currid);
       var $hidval=$.trim($hid.attr("code"));
	   if($hid.length>0){
		   $data=$hid.data("selectdata"+$currid);

           if($hidval!=""&&(typeof($data)=="undefined"||$data=="")){
                $data=typeof($data)=="undefined"?"":$data;
                
                showPallNode(dataView,$hidval,function(item){
                    if(item!=""){
                        $data=$data==""?item:item+","+$data
                    }
                });
                $data+=$data==""?$hidval:","+$hidval
           }
	   }
      
	   if(typeof($data)=="undefined"||$data==""){
		   ($currdiv.length>0)&&(showView(),!0)||createView();
	   }else{
		    //if($currdiv.is(":hidden")){//判断是否隐藏
            //}
            //$currdiv.html("");
		    //加载第一级数据
		    ($currdiv.length>0)&&(showView(),!0)||createView();
		    //加载选中的数据
		    var select=$data.split(",");
		    for(var j in select){
			    if(j!=0){
				    //showView();
			        //}else{
				    //j==0时，不加载第一级数据
                    //判断是否已存在级
                    if($(".menu-item li[class*='fmkli'][code='"+select[j]+"']").length==0){
				        showdata(select[j-1]);
                    }
			    }
			    if(!$("ul.menu-item li[code='"+select[j]+"']").hasClass("cur"))
			    {
				    $("ul.menu-item li[code='"+select[j]+"']").siblings("li").removeClass("cur");
				    $("ul.menu-item li[code='"+select[j]+"']").addClass("cur");
			    }
		    }
	   }
	},
	/**
    * 创建div
    * @method createView
    * @param 
    * @return 
    * @author: szj (2017-04-20 10:35:08)
    */
   createView=function(){
		var str='<div class="pop-menu" xplacement="'+$currid+'">';
		str+='</div>';
		$("body").append(str);
		
		($currdiv.length==0)&&function(){$currdiv=$('div[xplacement="'+$currid+'"]')}();
        $span.toggleClass("cur");//修改为向下的图标
		showdata("");
		showpos();
   },
   /**
    * 显示div
    * @method createView
    * @param 
    * @return 
    * @author: szj (2017-04-20 10:35:08)
    */
   showView=function(){
	   $currdiv.is(":hidden")&&function(){  
		   $currdiv.fadeIn(200);
           $span.toggleClass("cur");//修改为向下的图标
		   showdata("");
		   showpos();
	   }();
   },
   /**
    * 隐藏div
    * @method createView
    * @param 
    * @return 
    * @author: szj (2017-04-20 10:35:08)
    */
   hiddView=function(){
	   $currdiv.is(":visible")&&($currdiv.html(""),$span.toggleClass("cur"),$currdiv.fadeOut(200));
   },
   /**
    * 显示div位置
    * @method createView
    * @param 
    * @return 
    * @author: szj (2017-04-24 15:29:11)
    */
   showpos=function(){
	   if($currdiv.length==0){
		   $currdiv=$('div[xplacement="'+$currid+'"]');
	   }
	  /* var ent = event || window.event;
	   $currdiv.css("left", document.body.scrollLeft + ent.clientX + 1);
	   $currdiv.css("top", document.body.scrollLeft + ent.clientY + 20);*/
	   
	   var top= $e.offset().top;//获取元素所在top
	   var left=$e.offset().left;//获取元素所在left
	   var height=$e.height(); //获取元素高度
	   
	   //获取需要显示div的宽度
	   var divwidth=$currdiv.width();
	   var ulwidth=$(".menu-item").width();
	   divwidth=divwidth+ulwidth;
	   
	   //可视区域宽
	   var offsetWidth=$(window).width();
	   
	   var wh=offsetWidth-(left+divwidth);
	   if(wh<10){
		   left=left-ulwidth;
	   }
	   
	   $currdiv.css("left", left);
	   $currdiv.css("top", top + height+5);
   },
   /**
    * 数据绑定
    * @method showdata
    * @param {Stirng} obj 查询条件(唯一code)
    * @return 
    * @author: szj (2017-04-21 10:35:08)
    */
   showdata=function(obj){
	   var $li='';	//分类
	   var $sl='';	//含子类的样式
	   var $str='';	
	   
	  if(typeof(dataView)!="undefined")
	  { 
	      if (typeof (obj) == "undefined" || obj == "" || obj == null) {
	          $('div.pop-menu[xplacement!="' + $currid + '"]').remove();
			   //首次加载
			   $.each(dataView,function(i,item) {
				   $sl=(typeof(item.children)!="undefined"&&item.children.length>0)?"":"no";
				   $li+='<li class="fmkli '+$sl+'" code="'+item.code+'" val="'+item.value+'">'+item.label+'</li>';
			   });
			   //if($li!="")
			   $str+='<ul class="menu-item">'+$li+'</ul>';
		   }else{
			   $li='';
			   $sl='';
			   //根据code查询子类
			   showAllNode(dataView,obj,function(filterarray){
				   if(filterarray.length>0){
					   if(typeof(filterarray[0].children)!="undefined"&&filterarray[0].children.length>0){
						   $.each(filterarray[0].children,function(i,item) {
							   $sl=(typeof(item.children)!="undefined"&&item.children.length>0)?"":"no";
							   $li+='<li class="fmkli '+$sl+'" pcode="'+filterarray[0].code+'" code="'+item.code+'" val="'+item.value+'">'+item.label+'</li>';
						   });
					   }
				   }
			   });
			   showpos();
			   if($li!="")
				   $str+='<ul class="menu-item">'+$li+'</ul>';
		   }
	  }
	  ($currdiv.length>0)&&($currdiv.append($str),!0)||function(){
		   $('div.pop-menu[xplacement="'+$currid+'"]').append($str);
	   }();
   },
   /**
    * 递归遍历n维数组,向下查询
    * @module 
    * @author: szj (2017-04-21 10:35:08)
    */
   showAllNode=function(thedata,obj,callback){
	   var filterarray= $.grep(thedata,function(value){
           return value.code == obj;//筛选出等于obj的
	   });
	   $.each(thedata,function(i,item) {
	       if(filterarray.length==0){
	    	   if(typeof(item.children)!="undefined"&&item.children.length>0)
	    		   showAllNode(item.children,obj,callback);
	       }else{
	    	   callback(filterarray);
	    	   return false;
	       }
       });
	   return false;
   },
   /**
     * 根据Code查找当前节点以及父节点code
     *
     * @param 
     * @return
     */
   showPallNode=function(thedata,obj,callback){
        var parentNode = null;//父节点code

        //1.第一层 root 深度遍历整个JSON
        for (var i = 0; i < thedata.length; i++) {
            var oitem = thedata[i];
            //没有就下一个
            if (!oitem || !oitem.code) {
                continue;
            }
            //2.有节点就开始找，一直递归下去
             if (oitem.code == obj) {
                
             }else {
                //3.如果有子节点就开始找
                if (oitem.children) {
                    isCode(oitem.children,obj,function(filtera){
                        parentNode = oitem.code;//4.递归前，记录当前节点，作为parent 父亲 
                    })
                   showPallNode(oitem.children, obj,callback);//递归往下找
                }else{
                    continue;//跳出当前递归，返回上层递归
                }
            }
        }
        //如果没有找到父节点，则自己是root根，因为没有父亲
        if(!parentNode){
            parentNode = "";
        }
        callback(parentNode);
        return ;
    },
    /**
     * 根据Code查找是否有其节点code
     *
     * @param 
     * @return
     */
    isCode=function(oitemChil,obj,callback){
       var filterarray= $.grep(oitemChil,function(value){
           return value.code == obj;//筛选出等于obj的
	   });

       $.each(oitemChil,function(i,item) {
	       if(filterarray.length==0){
	    	   if(typeof(item.children)!="undefined"&&item.children.length>0)
	    		   isCode(item.children,obj,callback);
	       }else{
	    	   callback(filterarray);
	    	   return false;
	       }
       });
        return false;
    },
   /**
    * 选中数据
    * @method selectedval
    * @param {Stirng} code,{Stirng} value,{Stirng} label
    * @return 
    * @author: szj (2017-04-21 10:35:08)
    */
   selectedval=function(code,value,label,selecddata){
	   var $hid=$("#hid_"+$currid);
	   if($hid.length==0){
		   //没有保存数据的hidden
		   $("#"+$currid).after('<input type="hidden" id="hid_'+$currid+'" code="'+code+'" value="'+value+'">');
		   $hid=$("#hid_"+$currid);
	   }else{
		 //存在保存数据的hidden
		 $hid.attr("code",code);
		 $hid.val(value);
	   }
	   $("#"+$currid).val(label);
	   //保存选中级联
	   $hid.removeData("selectdata"+$currid);
	   $hid.data("selectdata"+$currid,selecddata);
   },
   /**
    * 清空选中的数据
    * @method emptyVal
    * @author: szj (2017-04-21 10:35:08)
    */
   emptyVal=function(){
	   var $hid=$("#hid_"+$currid);
	   if($hid.length>0){
		   $hid.removeData("selectdata"+$currid);
		   $hid.attr("code","");
		   $hid.val("");
	   }
   }
   /**
    * 单击li事件
    * @module 
    * @author: szj (2017-04-20 10:35:08)
    */
   $(document).on("click","li.fmkli",function(e){
	   var $code=$(this).attr("code"); //选中的code
	   var $value=$(this).attr("val"); //选中的value
	   var $label=$(this).html();	//选中的文本
	   var $selecddata=''; //选中的联级
	   
	   //判断是否选中，没有选中
	   /*if(!$(this).hasClass("cur")){ }*/
	   if($.trim($(this).attr("class"))=="fmkli"||$.trim($(this).attr("class"))=="fmkli  cur"){
		   //存在下一级
		   $(this).closest("ul.menu-item").nextAll("ul.menu-item").remove();
		   //删除同级高亮显示
		   $(this).siblings("li").removeClass("cur");
		   //高亮显示选中的数据
		   $(this).addClass("cur");
		   showdata($code);//加载下一级数据
	   }else{
		   //没有下一级
		   
		   //删除之前存在的下一级
		   $(this).closest("ul.menu-item").nextAll("ul.menu-item").remove();
		   //删除同级高亮显示
		   $(this).siblings("li").removeClass("cur");
		   
		   $.each($("ul.menu-item li[class*='cur']"),function(i,obj){
			   var code=$(obj).attr("code");
			   $selecddata+=$selecddata==""?code:","+code;
		   });
		   //判断有没有当前元素的code
		   if($selecddata.indexOf(","+$code)==-1)
			   $selecddata+=$selecddata==""?$code:","+$code;
		   //选中的数据
		   selectedval($code,$value,$label,$selecddata);
		   hiddView();
	   }
   });
   /**
    * 双击li事件,选中
    * @module 
    * @author: szj (2017-04-24 15:29:59)
    */
   $(document).on("dblclick","li.fmkli",function(e){
	   var $code=$(this).attr("code"); //选中的code
	   var $value=$(this).attr("val"); //选中的value
	   var $label=$(this).html();	//选中的文本
	   var $selecddata=''; //选中的联级
	   //删除之前存在的下一级
	   $(this).closest("ul.menu-item").nextAll("ul.menu-item").remove();
	   //删除同级高亮显示
	   $(this).siblings("li").removeClass("cur");
	   //高亮显示选中的数据
	   $(this).addClass("cur");
	   
	   $.each($("ul.menu-item li[class*='cur']"),function(i,obj){
		   var code=$(obj).attr("code");
		   $selecddata+=$selecddata==""?code:","+code;
	   });
	   //选中的数据
	   selectedval($code,$value,$label,$selecddata);
	   hiddView();
   });
   /**
    * 单击div外部，隐藏div
    * @module 
    * @author: szj (2017-04-20 10:35:08)
    */
   $(document).on("click","body",function(e){
	    e = e ? e.target : window.event.srcElement;
	    var localName = e.localName || e.nodeName;
	    localName = localName.toLowerCase();
	    if ((localName == "li" && e.className.indexOf("fmkli") > -1) || (localName == "input" && $currid == e.id)) {
			 if(e.control){
				 $('div.pop-menu[xplacement!="'+$currid+'"]').remove();
				 return ;
			 }
		 }else if(typeof($currdiv)!="undefined"&&$currdiv.is(":visible")){
			 $currdiv.html("");
             $span.toggleClass("cur");//修改为向下的图标
			 $currdiv.fadeOut(200);
		 }
		 
	})
})(jQuery); 