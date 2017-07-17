using MicrosoftResearch.Infer.Maths;
using MicrosoftResearch.Infer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetToNewsCS.Model.Infrastructure
{

    /// <summary>
    /// 参考にしました：http://infernet.azurewebsites.net/docs/Standard%20LDA.aspx
    /// https://www.slideshare.net/tsubosaka/infernetlda
    /// </summary>
    public class LDA
    {
        public LDA(IEnumerable<string> documents, int sizeOfVocabulary, int numberOfLatentTopics, double alpha, double beta)
        {
            //パラメータベクトルの定義
            Range d = new Range(documents.Count()).Named("d");
            Range w = new Range(sizeOfVocabulary).Named("w");
            Range t = new Range(numberOfLatentTopics).Named("t");

            VariableArray<Vector> phi = Variable.Array<Vector>(t);
            var phiSparsity = Sparsity.ApproximateWithTolerance(1.0e-6);
            phi.SetSparsity(phiSparsity);
            phi[t] = Variable.DirichletSymmetric(t, alpha).ForEach(d);

            var numWordsInDocument = Variable.Array<int>(d).Named("numWordsInDocument");
            using()

            Range winD = new Range(numWordsInDocument[d]).Named("winD");
        }
    }
}
