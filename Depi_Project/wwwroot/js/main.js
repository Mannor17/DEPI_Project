/* Shared UI interactions */
(function(){
	function qs(sel, root=document){ return root.querySelector(sel); }
	function qsa(sel, root=document){ return Array.from(root.querySelectorAll(sel)); }

	// Tabs
	qsa('.tabs').forEach(group => {
		group.addEventListener('click', e => {
			const tab = e.target.closest('.tab');
			if(!tab) return;
			qsa('.tab', group).forEach(t => t.classList.remove('active'));
			tab.classList.add('active');
			const target = tab.getAttribute('data-target');
			if(target){
				const container = group.parentElement;
				qsa('[data-tab-panel]', container).forEach(p => p.style.display = p.getAttribute('data-tab-panel')===target ? 'block' : 'none');
			}
		});
	});

	// Modals
	qsa('[data-open-modal]').forEach(btn => btn.addEventListener('click', () => {
		const id = btn.getAttribute('data-open-modal');
		const m = qs(`#${id}`); if(m) m.classList.add('open');
	}));
	qsa('.modal').forEach(m => m.addEventListener('click', e => {
		if(e.target.classList.contains('modal') || e.target.closest('[data-close]')) m.classList.remove('open');
	}));

	// Simple charts (mini canvas charts without external libs)
	function drawSparkline(canvas, points, opts={}){
		if(!canvas) return; const ctx = canvas.getContext('2d');
		const w = canvas.width = canvas.clientWidth * devicePixelRatio;
		const h = canvas.height = canvas.clientHeight * devicePixelRatio;
		ctx.scale(devicePixelRatio, devicePixelRatio);
		const pad = 6; const max = Math.max(...points), min = Math.min(...points);
		ctx.clearRect(0,0,w,h); ctx.lineWidth = 2; ctx.strokeStyle = opts.color || '#10c4cf';
		ctx.beginPath();
		points.forEach((v,i)=>{
			const x = pad + i*(canvas.clientWidth-2*pad)/(points.length-1);
			const y = pad + (canvas.clientHeight-2*pad) * (1-(v-min)/(max-min||1));
			if(i===0) ctx.moveTo(x,y); else ctx.lineTo(x,y);
		});
		ctx.stroke();
	}
	qsa('[data-spark]').forEach(c => {
		const data = c.getAttribute('data-spark').split(',').map(Number);
		drawSparkline(c, data, { color: getComputedStyle(document.documentElement).getPropertyValue('--accent').trim() });
	});
})();
