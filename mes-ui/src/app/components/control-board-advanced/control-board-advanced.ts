import {AfterViewInit, Component, ElementRef, OnInit, Renderer2, ViewChild} from '@angular/core';
import {ControlBoardService} from '../../services/ControlBoardService';
import mx from '../../../mxgraph';                       // <- import values from factory()
import type {mxGraph, mxGraphModel} from 'mxgraph';  // <- import types only, "import type" is a TypeScript 3.8+ syntax

@Component({
  selector: 'app-control-board-advanced',
  imports: [],
  templateUrl: './control-board-advanced.html',
  styleUrl: './control-board-advanced.scss'
})
export class ControlBoardAdvanced implements OnInit, AfterViewInit {
  @ViewChild('graphContainer') graphContainer!: ElementRef;
  url = '/control-board-adv.html'
  @ViewChild('refElem') elem: ElementRef;



  constructor(private service: ControlBoardService) {
    if (mx.mxClient.isBrowserSupported()) {
      console.log('Yes! Yes!');
    }
  }

  graph: mxGraph;
  chartInfo = ''

  ngAfterViewInit(): void {

    this.elem.nativeElement.src = this.url;
    //     this.graph = new mx.mxGraph(this.graphContainer.nativeElement);
// let editor = new mx.mxEditor();
//     const xmlString = '<mxGraphModel><root><mxCell id="0"/><mxCell id="1" parent="0"/><mxCell id="4" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;" parent="1" source="2" target="3" edge="1"><mxGeometry relative="1" as="geometry"/></mxCell><object label="" sid="55555" id="2"><mxCell style="whiteSpace=wrap;html=1;aspect=fixed;" parent="1" vertex="1"><mxGeometry x="580" y="300" width="80" height="80" as="geometry"/></mxCell></object><mxCell id="8" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;" parent="1" source="3" target="7" edge="1"><mxGeometry relative="1" as="geometry"/></mxCell><object label="" sid="99999" id="3"><mxCell style="ellipse;whiteSpace=wrap;html=1;aspect=fixed;" parent="1" vertex="1"><mxGeometry x="630" y="10" width="80" height="80" as="geometry"/></mxCell></object><mxCell id="9" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;entryX=0.5;entryY=1;entryDx=0;entryDy=0;" parent="1" source="5" target="2" edge="1"><mxGeometry relative="1" as="geometry"/></mxCell><object label="" sid="9876543" id="5"><mxCell style="shape=cylinder3;whiteSpace=wrap;html=1;boundedLbl=1;backgroundOutline=1;size=15;" parent="1" vertex="1"><mxGeometry x="70" y="260" width="60" height="80" as="geometry"/></mxCell></object><mxCell id="7" value="Actor" style="shape=umlActor;verticalLabelPosition=bottom;verticalAlign=top;html=1;outlineConnect=0;" parent="1" vertex="1"><mxGeometry x="10" y="10" width="40" height="60" as="geometry"/></mxCell><mxCell id="10" value="" style="shape=flexArrow;endArrow=classic;startArrow=classic;html=1;" parent="1" edge="1"><mxGeometry width="50" height="50" relative="1" as="geometry"><mxPoint x="290" y="250" as="sourcePoint"/><mxPoint x="430" y="140" as="targetPoint"/></mxGeometry></mxCell><mxCell id="11" value="" style="shape=process;whiteSpace=wrap;html=1;backgroundOutline=1;" parent="1" vertex="1"><mxGeometry x="410" y="220" width="120" height="60" as="geometry"/></mxCell><mxCell id="12" value="" style="shape=orEllipse;perimeter=ellipsePerimeter;whiteSpace=wrap;html=1;backgroundOutline=1;" parent="1" vertex="1"><mxGeometry x="60" y="440" width="80" height="80" as="geometry"/></mxCell><mxCell id="14" value="" style="shape=callout;whiteSpace=wrap;html=1;perimeter=calloutPerimeter;" parent="1" vertex="1"><mxGeometry x="440" y="480" width="120" height="80" as="geometry"/></mxCell></root></mxGraphModel>'
//     console.log("!",xmlString);
//     const doc = mx.mxUtils.parseXml(xmlString);
//     console.log("Doc is...",doc)
//     const codec = new mx.mxCodec(doc);
//     codec.decode(doc.documentElement, this.graph.getModel());
//     console.log(editor)


  }

  ngOnInit(): void {
    //    this.service.getChart().subscribe(c => {
    //   this.chartInfo = c;
    //
    //   this.graph = new mx.mxGraph(this.graphContainer.nativeElement);
    //      let editor =  new mx.Editor();
    //      editor.graph = this.graph;
    //
    //      const doc = mx.mxUtils.parseXml(this.chartInfo).documentElement;
    //
    //      editor.graph.
    //   console.log(doc)
    //
    //
    // })
  }
}
